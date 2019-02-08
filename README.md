## Visualizing planetary systems in VR

Course: Virtual and Augmented Reality under Prof. Andrew Johnson at the University of Illinois at Chicago.  

## Project Description

The second project was assigned to us with the intention of giving us the experience of creating a virtual world from data and looking at it ‘outside in’. Basically, we had to develop a project which visualizes the planetary systems that are present in our universe in 2-Dimension as well as 3-Dimension. The project is developed using Unity 3D software and HTC VIVE.  

## Application Developed

The repository contains data on more than 5000 exoplanet systems. In all, there were around 3200 planetary systems which were parsed which were filtered further to get about 600 systems for visual representation. These 600 planetary systems are divided into 4 groups each consisting of 150 planetary systems.  

The main menu provides the user with a medium for interacting with the application. Moreover, the main menu has several sub-menus which allows the user to perform several tasks such as bringing him back to the default location(center), showing him a list of all the available systems, change simulation options(such as orbit scaling, planet revolution speed change or orbital size change) and close the menu. Further, the menu allows the user to select the planetary systems of his choice to view in 2-D and then he can select a system from the available system in 2-D to view it in 3-D. When we select a system to be plotted in 3-D from its 2-D view, it would be erased in the 2-D view to make space for a new system to be bought in. The 2-D side view can accommodate up to 9 systems (which includes the solar system) while the 3-D view can accommodate our solar system and 4 additional systems. It is important to note that our solar system can never be erased from the two views since it is a reference system with which we are comparing the other systems. All this interactions can be done with the help of a laser pointer which is usable with the help of right wand.  

While interacting with the menu, the application makes a ‘beep sound’ for very user action. This feature is especially helpful for the user to track his own actions as the sound acts as a feedback for each of the user’s action. Since the real space doesn’t have any sound, we decided that the menus would be the right place to add sounds to.  

While in the 3-D view, the user can click on the star to display all the information about the planetary system. This includes name of the planetary system, distance from our solar system in light years, the type of star, how the planets were discovered, and current identifying name for each planet.  

So, just as the right wand is responsible for helping the user in using the menu functionality, the left wand is responsible for helping the user move around in 3-D space. This can be done with the help of the touch-pad.  

The application also includes the relative locations of the other stars with respect to that of the sun. Each star’s distance is scaled using the same scaling function and it is located at an exact angle with respect to the sun as it is in the actual universe. There are over 600 stars that are represented in the 3-D view and each star has a label on its head that represents its name. Also, we can click on any of the stars to generate its 3-D view for comparison with our solar system.  

## How to use 

To build the project, use the steps:  

1. Click on File-> Build Settings… -> Select platform as ‘PC, Mac and Linux Standalone’-> Target Platform as ‘Windows’ -> Architecture as ‘x86_64’.  
2. Click on Build to build the project.  

To run the project on your desktop, use the following steps.  

1. Open the Unity3D (version 5.5) software.  
2. Go to File –> Open Project –> “Your Project name”.  
3. Click on the Play button to run the project.  

Alternatively, you can also start the project by running the Executable file directly.  

To use this project with HTC VIVE and HMD, you’ll need to follow the same steps mentioned above. Ensure that your VIVE controllers and Head Mounted Display is connected/synced with your computer.  
