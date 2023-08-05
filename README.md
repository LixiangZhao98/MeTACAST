MeTACAST: a family of Target- and Context-aware spatial selection techniques for 3D point cloud data in VR
======
This is a repository for demo and source code of the paper "MeTACAST: a family of Target- and Context-aware spatial selection techniques for 3D point cloud data in VR", presented at [IEEE Visualization 2023](https://ieeevis.org/year/2023/welcome "VIS2023") and published in the journal [IEEE Transactions on Visualization and Computer Graphics](https://ieeexplore.ieee.org/xpl/RecentIssue.jsp?punumber=2945 "TVCG")

# Project website
![MeTACAST_Image][image]
[image]:/Assets/my/pic/MeTACAST.png
[MeTACAST](https://tobias.isenberg.cc/VideosAndDemos/Zhao2024MTC "MeTACAST")

# Requirement
* [Unity3D](https://unity3d.com/get-unity/download "Unity download") (Recommended version [2020.3.38f1](https://unity.cn/releases/lts/2020 "Unity3D 2020.3.38f1"))
* [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647 "SteamVR Plugin")
* [VIVE Input Utility](https://assetstore.unity.com/packages/tools/integration/vive-input-utility-64219 "VIVE Input Utility")

# Run the code
1. create a new Unity 3D project
2. install [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647 "SteamVR Plugin") and [VIVE Input Utility](https://assetstore.unity.com/packages/tools/integration/vive-input-utility-64219 "VIVE Input Utility") in the project
3. downlaod codes through zip from the repository
4. copy the `Assets/my` folder in this repository to the `Assets` folder of the unity project
5. run `Assets/my/Scenes/demo.unity` in the Unity Editor after connecting the VR devices

# Control
* hold `trigger` on the right controller to input
* use `joysticker/touchpad` on the right controller
* switch selection techniques and datasets on UI in VR or on the inspector of Unity Editor (click the gameobject `script/RunTimeController` in Hierarchy)