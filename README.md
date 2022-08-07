# MVRS-Skybox-Tools
Tools to create Skyboxes for Mobile VR Station (Quest)

## How to use

1. Download this repo to your local computer.
2. Download Unity and Open this reposatory as a project.
3. There are two different paths, 2D and 3D skyboxes, either navigate to /Assets/Input/2D or /Assets/Input/3D from within unity.
3.1 There is a info.json file.  You should open it and change some of the details.
4. The material is already setup, just replace the image.jpg with a image that you would want to see.  It should be a 360 "Spherical" image and in the case of 3D it should be Over/Under style.
5. If you click on the Sky material it should preview your skybox.  You may need to mess with the rotation adjustement to make sure forward is in the right direction.
6. Now right click in the asset menu and at the bottom choose Build 2D Skybox or Build 3D Skybox.  You may need to press the button twice, sometimes it just stops.
7. Now in the output folder /Assets/Output/2D or /Assets/Output/3D you should find a file skybox.skyzip.  That is your skybox file that should be deployed to headset.
