<h1 align="center" style="border-bottom: none;">Unity package template📦 </h1>
<h3 align="center">Fully automated version management and package publishing</h3>
<p align="center">
  <a href="https://github.com/semantic-release/semantic-release/actions?query=workflow%3ATest+branch%3Amaster">
    <img alt="Build states" src="https://github.com/semantic-release/semantic-release/workflows/Test/badge.svg">
  </a>
  <a href="https://github.com/semantic-release/semantic-release/actions?query=workflow%3ATest+branch%3Amaster">
    <img alt="semantic-release: angular" src="https://img.shields.io/badge/semantic--release-angular-e10079?logo=semantic-release">
  </a>
  <a href="LICENSE">
    <img alt="License" src="https://img.shields.io/badge/License-MIT-blue.svg">
  </a>
</p>
<p align="center">
  <a href="package.json">
    <img alt="Version" src="https://img.shields.io/github/package-json/v/OpenSourceUnityPackage/AxialSymmetryTerrain">
  </a>
  <a href="#LastActivity">
    <img alt="LastActivity" src="https://img.shields.io/github/last-commit/OpenSourceUnityPackage/AxialSymmetryTerrain">
  </a>
</p>

## What is it ?
Terrain symmetry allow you to apply basic axial symmetry to a scene with a terrain.
This feature allows you to create half of the terrain and generate the second part thanks to axial symmetry (useful in terms of game design).

<p align="center">
    <img src="https://user-images.githubusercontent.com/55276408/160634370-b8c2efd4-02e8-46b7-8323-772c02151dd1.png" alt="image" />
</p>

- 1: Create scene with your terrain (not wrapped in game object). This scene is the half of the map, so if you add camera, it will be duplicate.
- 2: Create new scene and add in game object the script "AxialSymmetryTerrain".

<p align="center">
    <img src="https://user-images.githubusercontent.com/55276408/160634470-7398405e-b0c0-49a9-b35b-c030608964ed.png" alt="image" />
</p>

- 3: Click on open and select your map.
- 4: Choose the type of symmetry that you want and then click on export to export the complete map. (you can editor half map in this scene)
- 5: Tada ! Your map is complete and ready to be use 🎉


