[VIS2023] MeTACAST: Target- and Context-aware Spatial Selection in VR
======

| ![MeTACAST_Image](https://github.com/LixiangZhao98/MeTACAST/blob/master/Assets/MeTACAST/pic/MeTACAST.png "MeTACAST_Image") |
|:--------------------------------------------------------------------------------------------------------------------------:|

| ![GIF](https://github.com/LixiangZhao98/asset/blob/master/Publications/Videos/MeTACASTvideo_final.gif "Image") |
|:-------------------------------------------------------------------------------------------------------------:|

[Paper](https://doi.org/10.1109/TVCG.2023.3326517) | [Video](https://www.youtube.com/watch?v=R_WRfzgnOAM&t=1s "Video") | [MeTACAST user study data and R scripts](https://github.com/LixiangZhao98/MeTACAST-study "MeTACAST-study")

MeTACAST is a set of selection techniques for particle data in a VR environment. Users can select groups of particles with natural 6DOF point and stroke input. Refer to the [video](https://www.youtube.com/watch?v=R_WRfzgnOAM&t=1s "Video") for a quick overview. For more details, please refer to our [paper](https://doi.org/10.1109/TVCG.2023.3326517) (MeTACAST: Target- and Context-aware Spatial Selection in VR), which was presented at [IEEE Visualization 2023](https://ieeevis.org/year/2023/welcome "VIS2023") and published in [IEEE Transactions on Visualization and Computer Graphics](https://ieeexplore.ieee.org/xpl/RecentIssue.jsp?punumber=2945 "TVCG").

Pull requests and issues are welcome. If you find this project useful, please consider giving it a star.

## Paper Information

L. Zhao, T. Isenberg, F. Xie, H. -N. Liang and L. Yu, "MeTACAST: Target- and Context-Aware Spatial Selection in VR," in IEEE Transactions on Visualization and Computer Graphics, vol. 30, no. 1, pp. 480-494, Jan. 2024, doi: [10.1109/TVCG.2023.3326517](https://doi.org/10.1109/TVCG.2023.3326517); open-access versions are available at [arXiv](https://arxiv.org/abs/2308.03616).

## BibTeX

```bibtex
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

## Related Repositories

- [PointCloud-Visualization-Tool](https://github.com/LixiangZhao98/PointCloud-Visualization-Tool "PointCloud-Visualization-Tool")

## Requirements

- Platform: Windows
- Unity: `2020.3.38f1` and `2021.3.19f1` have been tested; this workspace has also been opened with `2022.3.36f1`
- VR headsets: `HTC Vive Pro`, `HTC Vive Pro 2`, `HTC Vive Pro Eye`, and `Valve Index` have been tested
- Runtime: [Steam](https://store.steampowered.com/ "Steam") and SteamVR
- Unity packages:
  - [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647 "SteamVR Plugin")
  - [VIVE Input Utility](https://assetstore.unity.com/packages/tools/integration/vive-input-utility-64219 "VIVE Input Utility")

Meta/Oculus devices are not supported out of the box. To use them, import the required Oculus/OpenXR packages and revise the controller input logic.

## Unity Setup

1. Clone this repository or download the archive from [the master branch](https://github.com/LixiangZhao98/MeTACAST/archive/refs/heads/master.zip "archive").
2. Download Unity Hub and create a new Unity project. If you are new to Unity, refer to sections 1-5 of this [Unity setup tutorial](https://github.com/LixiangZhao98/asset/blob/master/Tutorial/Unity_Setup_General.pdf).
3. Install the SteamVR Plugin and VIVE Input Utility in the new project. If you are not familiar with package installation, refer to section 7 of the same [tutorial](https://github.com/LixiangZhao98/asset/blob/master/Tutorial/Unity_Setup_General.pdf).
4. Enable OpenVR and set `Stereo Rendering Mode` to `Multi Pass` in `Edit > Project Settings > XR Plug-in Management > OpenVR`.
5. Copy both of these folders into your Unity project's `Assets` folder:
   - `Assets/MeTACAST`
   - `Assets/PointCloud-Visualization-Tool`
6. Install SteamVR on your PC, connect the VR headset, and start SteamVR.
7. Open the scene `Assets/MeTACAST/scene/MeTACAST.unity` in Unity and press Play.

## Unity Controls

- Hold `trigger` on the right controller to start selection, then release `trigger` to confirm selection.
- Use the right controller `joystick` or `touchpad` to adjust the density threshold for MeTABrush and MeTAPaint.
- Hold `grip` to erase the selected region.
- Use the `undo`, `redo`, and `reset` UI buttons near the red sphere on the right controller.
- To switch selection techniques, density-grid resolution, or datasets in the Unity editor, select the `data/Kernel Density Estimator` GameObject in the Hierarchy and adjust the values in the Inspector.

## Executable Demo

1. Download `MeTACAST_Demo.zip` from the [Releases](https://github.com/LixiangZhao98/MeTACAST/releases "Releases") page.
2. Connect a PC-powered VR headset and start SteamVR.
3. Run `MeTACAST_Demo.exe`.

### Demo Controls

- Open the menu with `Menu`, `A`, or `B` on the right controller to switch selection techniques and datasets.
- Hold `trigger` on the right controller to start selection, then release `trigger` to confirm selection.
- Use the right controller `joystick` or `touchpad` to adjust the density threshold for MeTABrush and MeTAPaint.
- Hold `grip` to erase the selected region.
- Use the `undo`, `redo`, and `reset` UI buttons near the red sphere on the right controller.

## Project Website

[https://yulingyun.com/MeTACAST/](https://yulingyun.com/MeTACAST/ "MeTACAST")

## User Study Data and R Scripts

- [MeTACAST-study](https://github.com/LixiangZhao98/MeTACAST-study "MeTACAST-study")
- [OSF](https://osf.io/dvj9n/ "osf")

## Thanks

Many thanks to the authors of the open-source repository [unity-marching-cubes-gpu](https://github.com/pavelkouril/unity-marching-cubes-gpu "unity-marching-cubes-gpu").
