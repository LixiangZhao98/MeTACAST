MeTACAST: Target- and Context-aware Spatial Selection in VR
======
![MeTACAST_Image](https://github.com/LixiangZhao98/MeTACAST/blob/master/Assets/my/pic/MeTACAST.png "MeTACAST_Image")


MeTACAST is a set of selection techniques for particle data in VR environment. Users can select a group of particles with natural and simple 6DOF point/stroke input. Refer to the [Video](https://www.youtube.com/watch?v=R_WRfzgnOAM&t=1s "Video") for a quick review. For more details, please refer to our [Paper](https://doi.org/10.1109/TVCG.2023.3326517) (MeTACAST: Target- and Context-aware Spatial Selection in VR), which is presented on [IEEE Visualization 2023](https://ieeevis.org/year/2023/welcome "VIS2023") and published in the journal [IEEE Transactions on Visualization and Computer Graphics](https://ieeexplore.ieee.org/xpl/RecentIssue.jsp?punumber=2945 "TVCG").\
Any pull requests and issues are welcome. If you find it useful, could you please leave a star here? Thanks in advance.

## Related GitHub Repos
[PointCloud-Visualization-Tool](https://github.com/LixiangZhao98/PointCloud-Visualization-Tool "PointCloud-Visualization-Tool")

# How to use
MeTACAST is developed with Unity3D on `Windows` platform. \
Unity version requirement: Version `2020.3.38f1` and `2021.3.19f1` have been tested.\
VR headset requirement: `HTC Vive Pro/Pro2/Pro eye` and `Valve Index` have been tested. \
(Oculus is not supported currently, it wil be added in the future. If you want to use Oculus you need to import Oculus package and revise the input logic.)

## Install MeTACAST:
- Download Unity3D  and Create a new project. Here is a tutorial ([Unity3D Setup](https://github.com/LixiangZhao98/MeTACAST/blob/master/Assets/my/file/UnitySetup.pdf "Unity Setup")).
- Install [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647 "SteamVR Plugin") and [VIVE Input Utility](https://assetstore.unity.com/packages/tools/integration/vive-input-utility-64219 "VIVE Input Utility"). Please refer to sec.6 in [Unity3D Setup](https://github.com/LixiangZhao98/MeTACAST/blob/master/Assets/my/file/UnitySetup.pdf "Unity Setup") to install packages.
- Set the `Stereo Rendering Mode` to `Multi Pass`. In the Unity editor, find `Edit/project setting/XR Plug-in Management/OpenVR/Stereo Rendering Mode/Multi Pass`.
- Copy the `Assets/my` folder in this repo and place it under the `Assets` folder of your project.
- Install [Steam](https://store.steampowered.com/ "Steam") on your PC and download `SteamVR` on Steam.
- Connect the VR headset with PC and start `SteamVR`.
- Back to Unity and play `Assets/my/Scenes/demo.unity`.

## Control
* Hold `trigger` on the right controller to start selection, release `trigger` to confirm selection
* Use `joysticker/touchpad` on the right controller to adjust density threshold (MeTABrush and MeTAPaint)
* Hold `grip` to erase the selected region.
* Hit the `undo`, `redo` and `reset` on the ui (above the data on the left controller) by the red sphere in your right controller to undo, redo and reset
* To switch `selection techniques`, `resolution of the density grid` and `datasets`, click the gameobject `script/RunTimeController` in Hierarchy and switch them in the inspector window


<!-- # MeTACAST Demo
## Run the demo
1. the demo only tests on `windows` platform
2. download `MeTACAST_Demo.zip` from Releases and run `MeTACAST_Demo.exe` after connecting the PC-Powered VR Headsets 

## Control
* open the menu with `Menu`, `AKey` or `Bkey` on the right controller to switch `selection techniques` and `datasets`
* other operations and controls are the same as abovementioned -->




# Project website
[https://yulingyun.com/MeTACAST/](https://yulingyun.com/MeTACAST/ "MeTACAST")

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
```

# Thanks
Many thanks to the authors of open-source repository:
[unity-marching-cubes-gpu](https://github.com/pavelkouril/unity-marching-cubes-gpu "unity-marching-cubes-gpu")

