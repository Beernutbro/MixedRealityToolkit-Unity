﻿using System.Collections;
using System.IO;
using UnityEngine;

namespace GLTF
{
    class GLTFComponentStreamingAssets : MonoBehaviour
    {
        [Tooltip("This file should be in the StreamingAssets folder. Please include the file extension.")]
        public string GLTFName;
        public bool Multithreaded = true;

        public int MaximumLod = 300;

        public Shader GLTFStandard;
        public Shader GLTFConstant;

        [HideInInspector]
        public byte[] GLTFData;

        private void Start()
        {
            if (GLTFName.Length > 0)
            {
                if (File.Exists(Path.Combine(Application.streamingAssetsPath, GLTFName)))
                {
                    GLTFData = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, GLTFName));
                    StartCoroutine(LoadModel());
                }
                else
                {
                    Debug.Log("The glTF file specified on " + name + " does not exist in the StreamingAssets folder.");
                }
            }
        }

        public IEnumerator LoadModel()
        {
            var loader = new GLTFByteArrayLoader(
                GLTFData,
                gameObject.transform
            );
            loader.SetShaderForMaterialType(GLTFLoader.MaterialType.PbrMetallicRoughness, GLTFStandard);
            loader.SetShaderForMaterialType(GLTFLoader.MaterialType.CommonConstant, GLTFConstant);
            loader.Multithreaded = Multithreaded;
            loader.MaximumLod = MaximumLod;
            yield return loader.Load();
        }
    }
}