import open3d as o3d
import numpy as np
import time
import copy
from plyfile import PlyData, PlyElement
import os

# Path to the config file
#config_path = os.path.join(os.path.dirname(__file__), "..", "ros_meta_quest", "Assets", "gaussian_splatting_config.txt")

# Read the gaussian-splatting path from the first line of the config file
#with open(config_path, "r") as file:
    #gaussian_splatting_path = file.readline().strip()

# Construct the required file paths
#input_ply = os.path.join(gaussian_splatting_path, "output", "test_data", "point_cloud", "iteration_15000", "point_cloud.ply") 
#output_ply = os.path.join(gaussian_splatting_path, "transformed_point_cloud.ply")

input_ply = os.path.join(os.path.dirname(__file__), "splat_output", "point_cloud", "iteration_15000", "point_cloud.ply") #maybe slashes wrong way around?
output_ply = os.path.join(os.path.dirname(__file__), "splat_output", "transformed_point_cloud.ply")

print(input_ply) # should be gaussian_splatting_setup/splat_output/point_cloud/iteration_15000/point_cloud.ply
print(output_ply)# should be gaussian_splatting_setup/splat_output/transformed_point_cloud.ply

# Read the PLY file
#ply_data = PlyData.read("C:/Users/takuy/gaussian-splatting/output/test_data/point_cloud/iteration_15000/point_cloud.ply")
ply_data = PlyData.read(input_ply)

# Extract the point cloud data
vertex_data = ply_data['vertex']
points = np.vstack([vertex_data['x'], vertex_data['y'], vertex_data['z']]).T

# Extract additional attributes
attributes = {
    'scale_0': vertex_data['scale_0'],
    'scale_1': vertex_data['scale_1'],
    'scale_2': vertex_data['scale_2'],
    'opacity': vertex_data['opacity'],
    'rot_0': vertex_data['rot_0'],
    'rot_1': vertex_data['rot_1'],
    'rot_2': vertex_data['rot_2'],
    'rot_3': vertex_data['rot_3'],
    'f_dc_0': vertex_data['f_dc_0'],
    'f_dc_1': vertex_data['f_dc_1'],
    'f_dc_2': vertex_data['f_dc_2'],
}

for attribute in attributes:
    print(attribute, vertex_data[attribute], vertex_data[attribute].size)

# Add a column of 1s to convert the Nx3 points to Nx4 (homogeneous coordinates)
num_points = points.shape[0]
ones_column = np.ones((num_points, 1))
points_homogeneous = np.hstack((points, ones_column))
'''
# Define the transformation matrix (4x4)
Tb_C_EE = np.array([
    [-0.01196293, -0.99805358, 0.06136113, 0.03266323],
    [0.99992666, -0.01223338, -0.00403371, -0.04593601],
    [0.00477642, 0.06130719, 0.99810752, -0.04024424],
    [0.0, 0.0, 0.0, 1.0]
])

Rc_e = Tb_C_EE[:3, :3]
Vc_e = Tb_C_EE[:3, 3]

Tb_B_EE = np.array([
    [0.84, 0.03, 0.54, 0.36],
    [0.03, -1.0, 0.0, 0.0],
    [0.54, 0.02, -0.84, 0.36],
    [0.0, 0.0, 0.0, 1.0]
])

Tb_EE_B = np.linalg.inv(Tb_B_EE)

Re_b = Tb_EE_B[:3, :3] #Tb_EE_B
Ve_b = Tb_EE_B[:3, 3]

Rb_c = Re_b @ Rc_e
Vb_c = (Re_b @ Vc_e) + Ve_b

Vb_c = Vb_c.reshape(3, 1)

matrix_3x4 = np.hstack((Rb_c, Vb_c))
bottom_row = np.array([[0.0, 0.0, 0.0, 1.0]])

transformation_final = np.vstack((matrix_3x4, bottom_row))
'''

scaling_factor = 0.5
translation_distance_x = -4
translation_distance_y = 4
translation_distance_z = -5

scaling_matrix = np.array([
    [scaling_factor, 0, 0, 0],
    [0, scaling_factor, 0, 0],
    [0, 0, scaling_factor, 0],
    [0, 0, 0, 1]
])

# Define the rotation angles (in degrees)
theta_x = np.radians(60)  # Rotation around x-axis (convert degrees to radians)
theta_y = np.radians(45)  # Rotation around y-axis
theta_z = np.radians(45)  # Rotation around z-axis

# Define the rotation matrix around the x-axis
rotation_matrix_x = np.array([
    [1, 0, 0, 0],
    [0, np.cos(theta_x), -np.sin(theta_x), 0],
    [0, np.sin(theta_x), np.cos(theta_x), 0],
    [0, 0, 0, 1]
])

# Define the rotation matrix around the y-axis
rotation_matrix_y = np.array([
    [np.cos(theta_y), 0, np.sin(theta_y), 0],
    [0, 1, 0, 0],
    [-np.sin(theta_y), 0, np.cos(theta_y), 0],
    [0, 0, 0, 1]
])

# Define the rotation matrix around the z-axis
rotation_matrix_z = np.array([
    [np.cos(theta_z), -np.sin(theta_z), 0, 0],
    [np.sin(theta_z), np.cos(theta_z), 0, 0],
    [0, 0, 1, 0],
    [0, 0, 0, 1]
])

translation_matrix = np.array([
    [1, 0, 0, translation_distance_x],
    [0, 1, 0, translation_distance_y],
    [0, 0, 1, translation_distance_z],
    [0, 0, 0, 1]
])

# Combine the transformations: Translation * Rotation * Scaling
#transformation_final = translation_matrix @ rotation_matrix_x @ scaling_matrix
transformation_final = translation_matrix @ rotation_matrix_x
#transformation_final = np.identity(4)

# Apply the transformation to the point cloud
transformed_points_homogeneous = (transformation_final @ points_homogeneous.T).T

# Drop the homogeneous coordinate to get back Nx3 points
transformed_points = transformed_points_homogeneous[:, :3]

# Create vertex array for PLY with additional fields from original
vertices = np.array([
    (p[0], p[1], p[2], 
     attributes['scale_0'][i], attributes['scale_1'][i], attributes['scale_2'][i], 
     attributes['opacity'][i], 
     attributes['rot_0'][i], attributes['rot_1'][i], attributes['rot_2'][i], attributes['rot_3'][i], 
     attributes['f_dc_0'][i], attributes['f_dc_1'][i], attributes['f_dc_2'][i]
    ) 
    for i, p in enumerate(transformed_points)
], dtype=[
    ('x', 'f4'), ('y', 'f4'), ('z', 'f4'),
    ('scale_0', 'f4'), ('scale_1', 'f4'), ('scale_2', 'f4'),
    ('opacity', 'f4'),
    ('rot_0', 'f4'), ('rot_1', 'f4'), ('rot_2', 'f4'), ('rot_3', 'f4'),
    ('f_dc_0', 'f4'), ('f_dc_1', 'f4'), ('f_dc_2', 'f4')
])

# Create a PlyElement for the transformed points
el = PlyElement.describe(vertices, 'vertex')

# Write the transformed point cloud to a new PLY file
#PlyData([el], text=True).write('C:/Users/takuy/gaussian-splatting/transformed_point_cloud.ply')
PlyData([el], text=True).write(output_ply)