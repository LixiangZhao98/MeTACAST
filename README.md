MeTACAST: Target- and Context-aware spatial selection techniques for 3D point cloud data in VR
======
![MeTACAST_Image](https://github.com/LixiangZhao98/MeTACAST/blob/master/Assets/my/pic/MeTACAST.png "MeTACAST_Image")
This is a repository for demo and source code of the paper "MeTACAST: Target- and Context-aware spatial selection techniques for 3D point cloud data in VR", presented at [IEEE Visualization 2023](https://ieeevis.org/year/2023/welcome "VIS2023") and published in the journal [IEEE Transactions on Visualization and Computer Graphics](https://ieeexplore.ieee.org/xpl/RecentIssue.jsp?punumber=2945 "TVCG")

We propose three novel spatial data selection techniques for particle data in VR visualization environments, `MeTAPoint`, `MeTABrush` and `MeTAPaint` . They are designed to be target- and context-aware and be suitable for a wide range of data features and complex scenarios. Each technique is designed to be adjusted to particular selection intents: the selection of consecutive dense regions, the selection of filament-like structures, and the selection of clusters---with all of them facilitating post-selection threshold adjustment. These techniques allow users to precisely select those regions of space for further exploration---with simple and approximate 3D pointing, brushing, or drawing input---using flexible point- or path-based input and without being limited by 3D occlusions, non-homogeneous feature density, or complex data shapes.

Any pull requests and issues are welcome.
# MeTACAST Demo
## Run the demo
1. the demo only tests on `windows` platform
2. download `MeTACAST_Demo.zip` from Releases and run `MeTACAST_Demo.exe` after connecting the PC-Powered VR Headsets 

## Control
* open the menu with `Menu`, `AKey` or `Bkey` on the right controller to switch `selection techniques` and `datasets`
* other operations and controls are the same as abovementioned

# MeTACAST source code
## Requirement
* MeTACAST is based on the game engine Unity[Unity3D](https://unity3d.com/get-unity/download "Unity download") (Recommended version [2020.3.38f1](https://unity.cn/releases/lts/2020 "Unity3D 2020.3.38f1")).
* For the VR scene to work, the VR headset must be first configured and functional with SteamVR.
* [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647 "SteamVR Plugin") and [VIVE Input Utility](https://assetstore.unity.com/packages/tools/integration/vive-input-utility-64219 "VIVE Input Utility") are required in your Unity 3D project.

## Run the code
1. create a new Unity 3D project
2. install [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647 "SteamVR Plugin") and [VIVE Input Utility](https://assetstore.unity.com/packages/tools/integration/vive-input-utility-64219 "VIVE Input Utility") in the project
3. set the `Stereo Rendering Mode` to `Multi Pass` (Edit/project setting/XR Plug-in Management/OpenVR/Stereo Rendering Mode/Multi Pass)
4. clone the repo with git and copy the `Assets/my` folder in this repository to the `Assets` folder of the unity project
5. `Assets/my/Scenes/demo.unity` is the Virtual Reality (VR) scene.

## Control
* hold `trigger` on the right controller to start selection, release `trigger` to confirm selection
* use `joysticker/touchpad` on the right controller to adjust density threshold (MeTABrush and MeTAPaint)
* hold `grip` to erase the selected region.
* hit the `undo`, `redo` and `reset` on the ui (above the data on the left controller) by the red sphere in your right controller to undo, redo and reset
* switch `selection techniques`, `resolution of the density grid` and `datasets` on the inspector of Unity Editor (click the gameobject `script/RunTimeController` in Hierarchy)


# Project website
[https://yulingyun.com/MeTACAST/](https://yulingyun.com/MeTACAST/ "MeTACAST")

# Links to Paper
[ieeexplore](https://ieeexplore.ieee.org/document/10292508 "ieeexplore")

[arxiv](https://arxiv.org/abs/2308.03616 "arxiv")

# Video
[youtube](https://www.youtube.com/watch?v=R_WRfzgnOAM&t=1s "Video")

# User study data and R script
[osf](https://osf.io/dvj9n/ "osf")

# Citations
```bibtex
@article{zhao2023metacast,
  title={MeTACAST: Target-and Context-aware Spatial Selection in VR},
  author={Zhao, Lixiang and Isenberg, Tobias and Xie, Fuqi and Liang, Hai-Ning and Yu, Lingyun},
  journal={IEEE Transactions on Visualization and Computer Graphics},
  year={2023},
  publisher={IEEE}
}
}
```
# Relevant repository
The point cloud visualization is built based on [PointCloud-Visualization-Tool](https://github.com/LixiangZhao98/PointCloud-Visualization-Tool "PointCloud-Visualization-Tool")

# Thanks
Many thanks to the authors of open-source repository:
[unity-marching-cubes-gpu](https://github.com/pavelkouril/unity-marching-cubes-gpu "unity-marching-cubes-gpu")

