using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Diagnostics;
using System.Threading.Tasks;
using TMPro;
using System.Net;
using System.IO;
using UnityEditor;
using UnityEngine.VFX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst;
using UnityEngine.VFX.Utility;
using Unity.Mathematics;
using System.Runtime.InteropServices;
using SplatVfx;
using static UnityEngine.ParticleSystem;

//using static UnityEditor.Rendering.CameraUI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class GaussianSplattingTrigger : MonoBehaviour
{
    public InputActionReference rightTriggerActionReference; // Reference to your right trigger InputAction

    private bool hasRun = false;

    public GameObject m_VFXTEMPLATE;

    //[SerializeField]private TMPro.TMP_Text loadingText;
    public TMP_Text loadingText;


    /*private void Start()
    {
        AddSplatToScene();
    }*/


    void OnEnable()
    {
        // Enable the input action
        if (rightTriggerActionReference != null && rightTriggerActionReference.action != null)
        {
            rightTriggerActionReference.action.performed += OnRightTriggerPressed;
            rightTriggerActionReference.action.Enable();
        }
    }

    void OnDisable()
    {
        // Disable the input action
        if (rightTriggerActionReference != null && rightTriggerActionReference.action != null)
        {
            rightTriggerActionReference.action.performed -= OnRightTriggerPressed;
            rightTriggerActionReference.action.Disable();
        }
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext context)
    {
        if (!hasRun)
        {
            hasRun = true;
            UnityEngine.Debug.Log("trigger pressed");
            ShowLoadingText("Training gaussian splatter...");
            //RunGaussianSplatting();
        }
    }

    async void RunGaussianSplatting()
    {
        // First Command: Training
        await Task.Run(() => RunProcess(
            @"cmd.exe",
            @"/c conda activate gaussian_splatting && python C:\Users\takuy\gaussian-splatting\train.py -s C:\Users\takuy\gaussian-splatting\test_data\test_vid -m C:\Users\takuy\gaussian-splatting\output\test_data --iterations 7000 && python C:\Users\takuy\gaussian-splatting\convert_ply_splat.py C:\Users\takuy\gaussian-splatting\output\test_data\point_cloud\iteration_7000\point_cloud.ply -o C:\Users\takuy\unity_meta_quest_ros\ros_meta_quest\Assets\output.splat"
        ));

        //AssetDatabase.Refresh();
        ShowLoadingText("Exporting splat file...");
        AddSplatToScene();
        HideLoadingText();
    }

    void RunProcess(string fileName, string arguments)
    {
        print(arguments);
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(psi))
        {
            process.OutputDataReceived += (sender, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    UnityEngine.Debug.Log("Output: " + e.Data);
                }
            };
            process.ErrorDataReceived += (sender, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    UnityEngine.Debug.LogError("Error: " + e.Data);
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            UnityEngine.Debug.Log("exited: " + process.ExitCode);

            //if (process.ExitCode != 0)
            //{
                //UnityEngine.Debug.LogError("Process exited with code: " + process.ExitCode);
            //}
        }
    }

    void AddSplatToScene()
    {
        RuntimeImporter splatImporter = new RuntimeImporter();
        SplatData data = splatImporter.ImportAsSplatData("C:/Users/takuy/unity_meta_quest_ros/ros_meta_quest/Assets/output.splat");
       // GameObject test = splatImporter.CreatePrefab(data);
        
        GameObject t = splatImporter.init(m_VFXTEMPLATE,data);

        GameObject vfxInstance = Instantiate(t);

        //(GameObject)PrefabUtility.InstantiatePrefab(t);
    }

    void ShowLoadingText(string message)
    {
        if (loadingText != null)
        {
            loadingText.text = message;
            loadingText.gameObject.SetActive(true);
        }
    }

    void HideLoadingText()
    {
        if (loadingText != null)
        {
            loadingText.gameObject.SetActive(false);
        }
    }
}
