MeTACAST: Target- and Context-aware spatial selection techniques for 3D point cloud data in VR
======
![MeTACAST_Image](https://github.com/LixiangZhao98/MeTACAST/blob/master/Assets/my/pic/MeTACAST.png "MeTACAST_Image")
This is a repository for demo and source code of the paper "MeTACAST: Target- and Context-aware spatial selection techniques for 3D point cloud data in VR", presented at [IEEE Visualization 2023](https://ieeevis.org/year/2023/welcome "VIS2023") and published in the journal [IEEE Transactions on Visualization and Computer Graphics](https://ieeexplore.ieee.org/xpl/RecentIssue.jsp?punumber=2945 "TVCG")

We propose three novel spatial data selection techniques for particle data in VR \revise{visualization} environments. They are designed to be target- and context-aware and be suitable for a wide range of data features and complex scenarios. Each technique is designed to be adjusted to particular selection intents: the selection of consecutive dense regions, the selection of filament-like structures, and the selection of clusters---with all of them facilitating post-selection threshold adjustment. These techniques allow users to precisely select those regions of space for further exploration---with simple and approximate 3D pointing, brushing, or drawing input---using flexible point- or path-based input and without being limited by 3D occlusions, non-homogeneous feature density, or complex data shapes. These new techniques are evaluated in a controlled experiment and compared with the Baseline method, a region-based 3D painting selection. Our results indicate that our techniques are effective in handling a wide range of scenarios and allow users to select data based on their comprehension of crucial features. Furthermore, we analyze the attributes, requirements, and strategies of our spatial selection methods and compare them with existing state-of-the-art selection methods to handle diverse data features and situations. Based on this analysis we provide guidelines for choosing the most suitable 3D spatial selection techniques based on the interaction environment, the given data characteristics, or the need for interactive post-selection threshold adjustment.
# Project website
[https://yulingyun.com/MeTACAST/](https://yulingyun.com/MeTACAST/ "MeTACAST")

# Paper
[https://ieeexplore.ieee.org/document/10292508](https://ieeexplore.ieee.org/document/10292508 "ieeexplore")

[https://arxiv.org/abs/2308.03616](https://arxiv.org/abs/2308.03616 "arxiv")

# Video
[https://www.youtube.com/watch?v=R_WRfzgnOAM&t=1s](https://www.youtube.com/watch?v=R_WRfzgnOAM&t=1s "Video")

# User study data and R script
[https://osf.io/dvj9n/](https://osf.io/dvj9n/ "osf")

# MeTACAST source code
## Requirement
* [Unity3D](https://unity3d.com/get-unity/download "Unity download") (Recommended version [2020.3.38f1](https://unity.cn/releases/lts/2020 "Unity3D 2020.3.38f1"))
* [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647 "SteamVR Plugin")
* [VIVE Input Utility](https://assetstore.unity.com/packages/tools/integration/vive-input-utility-64219 "VIVE Input Utility")

## Run the code
1. create a new Unity 3D project
2. install [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647 "SteamVR Plugin") and [VIVE Input Utility](https://assetstore.unity.com/packages/tools/integration/vive-input-utility-64219 "VIVE Input Utility") in the project
3. set the `Stereo Rendering Mode` to `Multi Pass` (Edit/project setting/XR Plug-in Management/OpenVR/Stereo Rendering Mode/Multi Pass)
4. download codes through zip from the repository
5. copy the `Assets/my` folder in this repository to the `Assets` folder of the unity project
6. run `Assets/my/Scenes/demo.unity` in the Unity Editor after connecting the VR devices

## Control
* hold `trigger` on the right controller to start selection, release `trigger` to confirm selection
* use `joysticker/touchpad` on the right controller to adjust density threshold (MeTABrush and MeTAPaint)
* hold `grip` to erase the selected region.
* hit the `undo`, `redo` and `reset` on the ui (above the data on the left controller) by the red sphere in your right controller to undo, redo and reset
* switch `selection techniques`, `resolution of the density grid` and `datasets` on the inspector of Unity Editor (click the gameobject `script/RunTimeController` in Hierarchy)

# MeTACAST Demo
## Run the demo
1. the demo runs on windows platform
2. download `MeTACAST_Demo.zip` from Releases and run `MeTACAST_Demo.exe` after connecting the PC-Powered VR Headsets 

## Control
* open the menu with `Menu`, `AKey` or `Bkey` on the right controller to switch `selection techniques` and `datasets`
* other operations and controls are the same as abovementioned

