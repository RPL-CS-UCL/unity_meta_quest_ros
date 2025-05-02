from typing import Tuple, Sequence, Dict, Union, Optional, Callable
import numpy as np
import math
import torch
import torch.nn as nn
import torchvision
import collections
import cv2
import numpy as np
import matplotlib.pyplot as plt
import copy 
import pickle

from denoising_diffusion_pytorch import Unet, GaussianDiffusion
# from denoising_diffusion_pytorch.temporal import TemporalMixerUnet
from temporal_attention import TemporalUnet
import sys
from tqdm import tqdm

# from diffusers.schedulers.scheduling_ddpm import DDPMScheduler
# from diffusers.training_utils import EMAModel
# from diffusers.optimization import get_scheduler
# from tqdm.auto import tqdm
# import os


class diffusion_planner:
    def __init__(self, device='cuda'):
        self.H = 512 #128 #8

        self.obs_dim = 10
        self.device = device
        self.diffusion_iter = 1000
        self.model = TemporalUnet(
            horizon = self.H,
            transition_dim = self.obs_dim,
            cond_dim = self.H,
            dim = 128,
            dim_mults = (1, 2, 4, 8),
        ).to(torch.device(self.device))

        self.diffusion = GaussianDiffusion(
            self.model,
            channels = 1,
            image_size = (self.H, self.obs_dim),
            timesteps = self.diffusion_iter,   # number of steps
            loss_type = 'l1'    # L1 or L2
        ).to(torch.device(self.device))

        # hardcoded in the stats for now, obtained from the training data
        self.stats = {
            'min': -2.4091632296751815,
            'max': 1.9891460314338054
        }

    def load_weight(self, state_path):
        data = torch.load(state_path)

        self.step = data['step']
        # self.model.load_state_dict(data['model'])
        self.diffusion.load_state_dict(data['ema'])


    def plan(self, tracker_traj_obs, joint_initial, predict_horizon, batch = 1):
        # normalize the conditioning data
        tracker_traj_obs = normalize_data(tracker_traj_obs, self.stats)
        joint_initial = normalize_data(joint_initial, self.stats)


        # convert to torch tensor
        tracker_traj_obs = torch.from_numpy(tracker_traj_obs).to(self.device, dtype=torch.float32)
        joint_initial = torch.from_numpy(joint_initial).to(self.device, dtype=torch.float32)

        B = batch
        tracker_condition_length = tracker_traj_obs.shape[0]
        print("tracker_condition_length: ", tracker_condition_length)
        path_steps = predict_horizon + tracker_condition_length
        # print("path_steps: ", path_steps)        
        
        # self.diffusion = GaussianDiffusion(
        #     self.model,
        #     channels = 1,
        #     image_size = (path_steps, self.obs_dim),
        #     timesteps = self.diffusion_iter,   # number of steps
        #     loss_type = 'l1'    # L1 or L2
        # ).to(torch.device(self.device))

        with torch.no_grad():

            # initialize action from Guassian noise
            noisy_action = torch.randn(
                (B, path_steps, self.obs_dim), device=self.device)

            noisy_action[0,:tracker_condition_length,0:3] = tracker_traj_obs

            noisy_action[0,0,3:] = joint_initial
            # print("noisy_action shape: ", noisy_action.shape)

            naction = noisy_action.clone()

            # set up array of zeros to store intermediate predictions
            action_pred_intermediate = np.zeros((self.diffusion_iter + 1, path_steps, self.obs_dim))


            # empty mask
            shape = (B, self.H, self.obs_dim)
            mask  = torch.ones(*shape, device=self.device)[..., 0]
            # print(mask.shape)
            # mask[:, :tracker_condition_length] = 1
            # mask[:, 0, 3:] = 1
            # for i in range(self.diffusion_iter):
            for i in tqdm(reversed(range(0, self.diffusion_iter)), desc='sampling loop time step', total=self.diffusion_iter):

                # index for storing intermediate predictions starts from 0
                store_idx = self.diffusion_iter - i - 1
                # print("store_idx: ", store_idx)
                # save intermediate predictions for visualization:
                action_pred_intermediate[store_idx,:,:] = unnormalize_data(naction.detach().to('cpu').numpy()[0], stats=self.stats)

                # perform diffusion step
                naction = self.diffusion.p_sample(naction, mask, torch.full((B,), i, device=self.device, dtype=torch.long))

                # condtioning mask for the next step
                naction[0,:tracker_condition_length,0:3] = tracker_traj_obs

                naction[0,0,3:] = joint_initial
            # naction = self.diffusion.p_sample_loop(naction, mask)

            naction_np = naction.detach().to('cpu').numpy()
            action_pred = unnormalize_data(naction_np[0], stats=self.stats)
            action_pred_intermediate[-1,:,:] = action_pred

            return action_pred, action_pred_intermediate
        

def normalize_data(data, stats):
    # nomalize to [0,1]
    ndata = (data - stats['min']) / (stats['max'] - stats['min'])
    # normalize to [-1, 1]
    ndata = ndata * 2 - 1
    return ndata

def unnormalize_data(ndata, stats):
    ndata = (ndata + 1) / 2
    data = ndata * (stats['max'] - stats['min']) + stats['min']
    return data
