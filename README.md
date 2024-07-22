MeTACAST: Target- and Context-aware Spatial Selection in VR
======
![MeTACAST_Image](https://github.com/LixiangZhao98/MeTACAST/blob/master/Assets/MeTACAST/pic/MeTACAST.png "MeTACAST_Image")

[Paper](https://doi.org/10.1109/TVCG.2023.3326517) | [Video](https://www.youtube.com/watch?v=R_WRfzgnOAM&t=1s "Video") | [MeTACAST user study data and R scripts](https://github.com/LixiangZhao98/MeTACAST-study "MeTACAST-study")

MeTACAST is a set of selection techniques for particle data in VR environment. Users can select a group of particles with natural and simple 6DOF point/stroke input. Refer to the [Video](https://www.youtube.com/watch?v=R_WRfzgnOAM&t=1s "Video") for a quick review. For more details, please refer to our [Paper](https://doi.org/10.1109/TVCG.2023.3326517) (MeTACAST: Target- and Context-aware Spatial Selection in VR), which is presented on [IEEE Visualization 2023](https://ieeevis.org/year/2023/welcome "VIS2023") and published in the journal [IEEE Transactions on Visualization and Computer Graphics](https://ieeexplore.ieee.org/xpl/RecentIssue.jsp?punumber=2945 "TVCG").\
Any pull requests and issues are welcome. If you find it useful, could you please leave a star here? Thanks in advance.

## Paper information
L. Zhao, T. Isenberg, F. Xie, H. -N. Liang and L. Yu, "MeTACAST: Target- and Context-Aware Spatial Selection in VR," in IEEE Transactions on Visualization and Computer Graphics, vol. 30, no. 1, pp. 480-494, Jan. 2024, doi: [10.1109/TVCG.2023.3326517](https://doi.org/10.1109/TVCG.2023.3326517); open-access versions are available at [arXiv](https://arxiv.org/abs/2308.03616).

## BibTex
```
@article{Zhao:2024:MTC,
  author      = {Lixiang Zhao and Tobias Isenberg and Fuqi Xie and Hai-Ning Liang and Lingyun Yu},
  title       = {MeTACAST: Target- and Context-aware Spatial Selection in VR},
  journal     = {IEEE Transactions on Visualization and Computer Graphics},
  year        = {2024},
  volume      = {30},
  number      = {1},
  month       = jan,
  pages       = {480--494},
  doi         = {10.1109/TVCG.2023.3326517},
  shortdoi    = {10/gtnn25},
  doi_url     = {https://doi.org/10.1109/TVCG.2023.3326517},
  oa_hal_url  = {https://hal.science/hal-04196163},
  preprint    = {https://doi.org/10.48550/arXiv.2308.03616},
  osf_url     = {https://osf.io/dvj9n/},
  url         = {https://tobias.isenberg.cc/p/Zhao2024MTC},
  github_url  = {https://github.com/LixiangZhao98/MeTACAST},
  github_url2 = {https://github.com/LixiangZhao98/PointCloud-Visualization-Tool},
  github_url3 = {https://github.com/LixiangZhao98/MeTACAST-study},
  pdf         = {https://tobias.isenberg.cc/personal/papers/Zhao_2024_MTC.pdf},
  video       = {https://youtu.be/R_WRfzgnOAM},
}
```

## Project website
* https://yulingyun.com/MeTACAST/

## Related GitHub Repos
[PointCloud-Visualization-Tool](https://github.com/LixiangZhao98/PointCloud-Visualization-Tool "PointCloud-Visualization-Tool")

# How to use
MeTACAST is developed with Unity3D on `Windows` platform. \
Unity version requirement: Version `2020.3.38f1` and `2021.3.19f1` have been tested.\
VR headset requirement: `HTC Vive Pro/Pro2/Pro eye` and `Valve Index` have been tested. \
(Oculus is not supported currently, it wil be added in the future. If you want to use Oculus you need to import Oculus package and revise the input logic.)

## Install MeTACAST:
- Clone the repo with git lfs installed or download the archive [https://github.com/LixiangZhao98/MeTACAST/archive/refs/heads/master.zip](https://github.com/LixiangZhao98/MeTACAST/archive/refs/heads/master.zip "archive").
- Download Unity Hub and Create a new project. Please refer to sec.1 and sec.3 of [tutorial](https://github.com/LixiangZhao98/asset/blob/master/Tutorial/Unity_Setup_General.pdf) if you are new Unity user.
- Install [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647 "SteamVR Plugin") and [VIVE Input Utility](https://assetstore.unity.com/packages/tools/integration/vive-input-utility-64219 "VIVE Input Utility") in this new project. Please refer to sec.7 in [tutorial](https://github.com/LixiangZhao98/asset/blob/master/Tutorial/Unity_Setup_General.pdf) to install packages if you are not familiar with it.
- Set the `Stereo Rendering Mode` to `Multi Pass`. In the Unity editor, find `Edit/project setting/XR Plug-in Management/OpenVR/Stereo Rendering Mode/Multi Pass`.
- Copy the `Assets/MeTACAST` folder in this repo and place it under the `Assets` folder of your new project.
- Install [Steam](https://store.steampowered.com/ "Steam") on your PC and download `SteamVR`.
- Connect the VR headset with PC and start `SteamVR`.
- Back to Unity and play `Assets/MeTACAST/Scenes/demo.unity`.

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
[MeTACAST-study](https://github.com/LixiangZhao98/MeTACAST-study "MeTACAST-study")\
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

