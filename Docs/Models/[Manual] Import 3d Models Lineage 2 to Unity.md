### Description of how to import a 3d model into Unity  

![Import Npc](.img/1.png)

My Unity client does not have Fishing Guild Member Klufe NPC, now we want to add it to the client:  

1. We find the file Npcgrp.dat, read it using FileEditor and find our NPC his ID: `31562`. 
In the file we find the line with these IDs and cut out the following parameters: `mesh_name=LineageNPSs.a_fisher_Fog_m00`. 
texture_name:
   - `LineageNPCsTex.a_fisherA_Mhuman_m00_t00_b00`
   - `LineageNPCsTex.a_fisherA_Mhuman_m00_t00_b01`
   - `LineageNPCsTex.a_fisherA_Mhuman_m00_t00_f`
   - `LineageNPCsTex.a_fisherA_Mhuman_m00_t00_h`

From these parameters we got everything we need: the name of the mash file and the names of its textures.  

2. Now we need to find these files:  
We start the search for the original client:  
   * `Animations\LineageNpcs.ukx` (this is the folder with the necessary files)   
   * Launch the Gildor archiver and get the finished files: `LineageNpcs\SkeletalMesh\a_fisherA_Mhuman_m00.psk` - this is the NPC mesh  
   * Open Blender with the installed plugin for reading psk files  
   * Import this file and Export it to the format `a_fisherA_Mhuman_m00.fbx`
   * Return to Blender and click import .psa files; this file contains animations for our NPC: `LineageNpcs\MeshAnimation\a_fisherA_MHuman_anim.psa`

3. We found Mesh, now we need to pull out the textures for our NPC  
   * Find LineageNPCsTex this folder in the client     
   * Launch the Gildor archiver and get the finished files:    
   * I found all the files in the `LineageNpcsTex\Texture\` folder  
   * I copy all received files into a separate folder so that I can quickly find them in the future.  


4. Now we open Unity L2
   * Go to the folder `Assets\Resources\Data\Animations\LineageNPCs`
   * Create a new folder named `a_fisherA_Mhuman_m00`
   * Create a new folder named Model
   * Drag `a_fisherA_Mhuman_m00.fbx` to the Model folder  
   * Drag the appeared file into a blue scene to create a ready-made prefab with the same name but prefab extension
![Import Npc](.img/2.png)  
   * Double-click on the prefab and see this picture 
![Settings](.img/3.png)  
   * The height of the Npc is set incorrectly. We need to fix this by going to the CharacterController:  
![The height of the Npc](.img/4.png)  
   * Now we need to add textures to this mesh, take 4 of our texture files and drag them into the folder `Assets\Resources\Data\SysTextures\`
   * After copying, go to the folder `Assets\Resources\Data\SysTextures\Materials` and create 1 material `a_fisherA_MHuman_m00_t00_b00`
   * Left-click on `a_fisherA_MHuman_m00_t00_b00.mat` and drag a texture with the same name `a_fisherA_MHuman_m00_t00_b00.png` into the base map. Repeat this 4 times for all textures.
   * Create an empty object inside prefab GameObject → Create Empty and rename it to `click_area`
   * Select `click_area` and add components: Cube Mesh Filter, Box Collider  
   * Select Layer → EntityClick  
   * In BoxCollider we set Scale `0.25`, `0.9`, `0.25`
   * You can start the game and watch our npc (we will also need to connect the animator to this npc, but that’s for another time!)  
