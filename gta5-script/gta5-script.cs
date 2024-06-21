using GTA;
using GTA.Native;
using GTA.Math;
using GTA.UI;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static gtamod;

public class gtamod : Script
{
    private Blip lsiaBlip;
    private Blip cayoBlip;
    private Blip NorthYlsiaBlip;
    private Blip NorthYBlip;
    //cayo stuff
    private Vector3 lsiaBlipLocation = new Vector3(-1167f, -2736f, 18.5f); // blip and marker at LSIA (to travel to cayo)
    private Vector3 lsiaTeleportLocation = new Vector3(-1173f, -2750f, 14f); // tp location LSIA (when going from cayo to LSIA using marker)
    private Vector3 cayoBlipLocation = new Vector3(4496f, -4520f, 3f); // blip and marker at Cayo
    private Vector3 cayoTeleportLocation = new Vector3(4475f, -4462f, 4f); // tp location Cayo (travel to cayo marker at LSIA teleports here)
    //north yankton stuff
    private Vector3 NYlsiaBlip = new Vector3(-1042f, -2745.5f, 20f); // location of the blip(seen on the map) and marker at LSIA that teleports to north yankton
    private Vector3 NYlsiaTeleportLocation = new Vector3(-1038f, -2738f, 20f); //teleport FROM north yankton back to LSIA
    private Vector3 NY_BlipLocation = new Vector3(5479.7f, -5139.3f, 77f); // location of the marker at north yankton
    private Vector3 NY_TeleportLocation = new Vector3(5480f, -5118f, 80f); // north yankton´s LSIA marker will teleport here



    private bool isCayoPericoEnabled = false;
    private bool isFading = false; // Track if fading is in progress
    public bool isCayoProximityEnabled = true; //fucking piece of shit


    public gtamod()
    {

        // Register the Tick event
        Tick += OnTick;
        KeyDown += OnKeyDown;

        // Enable MP maps on startup
        EnableMpMaps();

        //Load Aircraft Carrier after MP maps
        AdditionalIPLs.RequestIpls();
        AdditionalIPLs.UnloadIpls();

        // Create blips
        CreateBlips();


        // Show startup message
        Notification.Show("Enabled more locations, enjoy.");
        


    }


    private void SetRandomWeatherForNY()
    {
        Random random = new Random();
        int weatherIndex = random.Next(3); // 0, 1, or 2 for SNOW, SNOWLIGHT, BLIZZARD

        switch (weatherIndex)
        {
            case 0:
                Function.Call(Hash.SET_OVERRIDE_WEATHER, "SNOW");
                break;
            case 1:
                Function.Call(Hash.SET_OVERRIDE_WEATHER, "SNOWLIGHT");
                break;
            case 2:
                Function.Call(Hash.SET_OVERRIDE_WEATHER, "BLIZZARD");
                break;
        }
    }

    private void ClearOverrideWeather()
    {
        Function.Call(Hash.SET_OVERRIDE_WEATHER, "CLEAR");
        Function.Call(Hash.CLEAR_OVERRIDE_WEATHER);
    }


    private void EnableMpMaps()
    {
        Function.Call((Hash)0x888C3502DBBEEF5, true); // Load MP maps
    }


    //***********************************************************************************************************
    //************************   locations   ********************************************************************
    //***********************************************************************************************************
    //not related to cayo or north yankton
    public class AdditionalIPLs
    {
        public static bool isLoaded = false;

        public static void RequestIpls()
        {
            if (!isLoaded)
            {
                // IPL list
                string[] iplNames = new string[]
                {
                //Aircraft carrier
                "hei_carrier",
                "hei_carrier_DistantLights",
                "hei_Carrier_int1",
                "hei_Carrier_int2",
                "hei_Carrier_int3",
                "hei_Carrier_int4",
                "hei_Carrier_int5",
                "hei_Carrier_int6",
                "hei_carrier_LODLights",
                "hei_bi_hw1_13_door",
                //Additional stuff
                "pink_diamond_set",
                "FINBANK",
                "FIBlobby",
                "facelobby",
                "post_hiest_unload",
                "RC12B_Destroyed",
                "RC12B_HospitalInterior",
                "cargoship",
                "SP1_10_real_interior",
                "v_carshowroom",
                "shutter_open",
                "shr_int",
                "csr_inMission",
                "DT1_03_Shutter",
                "hei_dlc_casino_door_broken",
                "linvader",
                "redCarpet",
                "smboat",
                "smboat_lod",
                "coronertrash",
                "Coroner_Int_On",
                "bkr_bi_hw1_13_int",
                "bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo",
                "bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo",
                "canyonriver01_traincrash",
                "railing_end"
                };

                // Loop through each IPL name and request it
                foreach (string iplName in iplNames)
                {
                    Function.Call<bool>(Hash.REQUEST_IPL, iplName);
                }

                isLoaded = true;
            }
        }

        public static void UnloadIpls()
        {
            if (isLoaded)
            {
                // IPL list to unload
                string[] iplNames = new string[]
                {
                "jewel2fake",
                "bh1_16_refurb",
                "facelobbyfake",
                "FIBlobbyfake"
                };

                // Loop through each IPL name and remove it
                foreach (string iplName in iplNames)
                {
                    Function.Call(Hash.REMOVE_IPL, iplName);
                }

                isLoaded = false;
            }
        }
    }


    public class LoadNY  //NY map hash 0x9133955F1A2DA957 , block creating waypoints hash 0x58FADDED207897DC
    {
        public static bool isLoaded = false;
        public static void RequestNY(gtamod mod)
        {
            if (!isLoaded)
            {
                string[] iplNames = new string[]
                {
            "prologue01",
            "prologue01c",
            "prologue01d",
            "prologue01e",
            "prologue01f",
            "prologue01g",
            "prologue01h",
            "prologue01i",
            "prologue01j",
            "prologue01k",
            "prologue01z",
            "plg_01",
            "plg_02",
            "plg_03",
            "plg_04",
            "plg_05",
            "plg_06",
            "plg_rd",
            "prologue02",
            "prologue03",
            "prologue03b",
            "prologue03_grv_cov",
            "prologue03_grv_cov_lod",
            "prologue_grv_torch",
            "prologue04",
            "prologue04b",
            "prologue04_cover",
            "des_protree_end",
            "des_protree_start",
            "prologue05",
            "prologue05b",
            "prologue06",
            "prologue06b",
            "prologue06_int",
            "prologue06_pannel",
            "prologue_occl",
            "prologuerd",
            "prologuerdb"
                };

                foreach (string iplName in iplNames)
                {
                    Function.Call(Hash.REQUEST_IPL, iplName);
                    
                    Function.Call((Hash)0x9133955F1A2DA957, true); //NY map
                    Function.Call(Hash.SET_ALLOW_STREAM_PROLOGUE_NODES, true); //nodes for the location - confirmed it works
                   //TODO: train spawning
                    

                    //enable zone - THIS DOES NOT CRASH FINALLY
                    int zoneId = Function.Call<int>(Hash.GET_ZONE_FROM_NAME_ID, "PrLog");
                    Function.Call(Hash.SET_ZONE_ENABLED, zoneId, true);
                    Function.Call(Hash.SET_MAPDATACULLBOX_ENABLED, "prologue", true);
                    Function.Call(Hash.SET_MAPDATACULLBOX_ENABLED, "Prologue_Main", true);

                    //enable paths - crash, NOTE: probably need to enable the north yankton zone in order to spawn snow vehicles, NOTE: ny zone is already enabled
                    //roads
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 5655.24f, -5142.23f, 61.78925f, 3679.327f, -4973.879f, 125.0828f, 192, false, true, true);
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 3691.211f, -4941.24f, 94.59368f, 3511.115f, -4869.191f, 126.7621f, 16, false, true, true);
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 3510.004f, -4865.81f, 94.69557f, 3204.424f, -4833.817f, 126.8152f, 16, false, true, true);
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 3186.534f, -4832.798f, 109.8148f, 3202.187f, -4833.993f, 114.815f, 16, false, true, true);
                    //1 side road had to be added manually
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 5493.3f, -5344.76f, 81.8f, 5483.187f, -5137.3f, 75.1f, 4, false, true, true);

                }


                mod.SetRandomWeatherForNY();
                isLoaded = true;
                mod.isCayoProximityEnabled = false; // Disable Cayo Perico proximity check when NY is loaded
            }
        }
      


        public static void UnloadNY(gtamod mod)
        {
            if (isLoaded)
            {
                string[] iplNames = new string[]
                {
            "prologue01",
            "prologue01c",
            "prologue01d",
            "prologue01e",
            "prologue01f",
            "prologue01g",
            "prologue01h",
            "prologue01i",
            "prologue01j",
            "prologue01k",
            "prologue01z",
            "plg_01",
            "plg_02",
            "plg_03",
            "plg_04",
            "plg_05",
            "plg_06",
            "plg_rd",
            "prologue02",
            "prologue03",
            "prologue03b",
            "prologue03_grv_cov",
            "prologue03_grv_cov_lod",
            "prologue_grv_torch",
            "prologue04",
            "prologue04b",
            "prologue04_cover",
            "des_protree_end",
            "des_protree_start",
            "prologue05",
            "prologue05b",
            "prologue06",
            "prologue06b",
            "prologue06_int",
            "prologue06_pannel",
            "prologue_occl",
            "prologuerd",
            "prologuerdb"
                };

                foreach (string iplName in iplNames)
                {

                    Function.Call(Hash.REMOVE_IPL, iplName);
                    Function.Call((Hash)0x9133955F1A2DA957, false);
                    Function.Call(Hash.SET_ALLOW_STREAM_PROLOGUE_NODES, false);
                    //Function.Call(Hash.SET_ZONE_ENABLED, "PrLog", false);
                    //Function.Call(Hash.SET_MAPDATACULLBOX_ENABLED, "prologue", false);
                    //Function.Call(Hash.SET_MAPDATACULLBOX_ENABLED, "Prologue_Main", false);
                    //Enable paths - no crash
                       //roads: bank - city
                    /*Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 5526.24f, -5137.23f, 61.78925f, 3679.327f, -4973.879f, 125.0828f, 192, false, true, true);
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 3691.211f, -4941.24f, 94.59368f, 3511.115f, -4869.191f, 126.7621f, 16, false, true, true);
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 3510.004f, -4865.81f, 94.69557f, 3204.424f, -4833.817f, 126.8152f, 16, false, true, true);
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 3186.534f, -4832.798f, 109.8148f, 3202.187f, -4833.993f, 114.815f, 16, false, true, true);*/
                    //roads: city - bank
                    


                    int zoneId = Function.Call<int>(Hash.GET_ZONE_FROM_NAME_ID, "PrLog");
                    Function.Call(Hash.SET_ZONE_ENABLED, zoneId, false);

                    //Function.Call(Hash.SWITCH_TRAIN_TRACK, "trains10.dat", false); // Enable Yankton train track
                    //Function.Call(Hash.SWITCH_TRAIN_TRACK, "trains12.dat", false); // Enable Yankton prologue mission train track



                }

                mod.ClearOverrideWeather();
                isLoaded = false;
                mod.isCayoProximityEnabled = true; // Re-enable Cayo Perico proximity check when NY is unloaded
            }
        }
    }


    // if player falls bellow Z: 30, tp him back to the NY_teleportlocation
    private void NorthYanktonPositionCheck()
    {
        if (LoadNY.isLoaded && Game.Player.Character.Position.Z < 30f)
        {
            // Check if player is in a vehicle
            if (Game.Player.Character.IsInVehicle())
            {
                // Get the vehicle the player is in
                Vehicle playerVehicle = Game.Player.Character.CurrentVehicle;
                // Teleport the vehicle and the player inside it
                playerVehicle.Position = NY_TeleportLocation;
            }
            else
            {
                // Teleport the player if not in a vehicle
                Game.Player.Character.Position = NY_TeleportLocation;
            }
        }
    }

    //***********************************************************************************************************
    //************************   Blips and markers   ************************************************************
    //***********************************************************************************************************

    private void CreateBlips()
    {
        // Create blip at original LSIA location (plane icon on the map)
        lsiaBlip = World.CreateBlip(lsiaBlipLocation);
        lsiaBlip.Sprite = BlipSprite.Airport;
        lsiaBlip.Color = BlipColor.Yellow;
        lsiaBlip.Name = "Cayo Perico";

        // Create blip at original Cayo Perico location (plane icon on the map)
        cayoBlip = World.CreateBlip(cayoBlipLocation);
        cayoBlip.Sprite = BlipSprite.Airport;
        cayoBlip.Color = BlipColor.Yellow;
        cayoBlip.Name = "Los Santos";
        cayoBlip.Alpha = 0; // Make it invisible initially

        //LSIA -> north yankton blip
        NorthYlsiaBlip = World.CreateBlip(NYlsiaBlip);
        NorthYlsiaBlip.Sprite = BlipSprite.Airport;
        NorthYlsiaBlip.Color = BlipColor.Blue;
        NorthYlsiaBlip.Name = "North Yankton";

        //north yankton -> LSIA blip
        NorthYBlip = World.CreateBlip(NY_BlipLocation);
        NorthYBlip.Sprite = BlipSprite.Airport;
        NorthYBlip.Color = BlipColor.Blue;
        NorthYBlip.Name = "Los Santos";
        NorthYBlip.Alpha = 0; //make it invisible if not on NY


    }

    private void DrawMarkers()
    {
        // Light blue color as in GTA Online (RGBA: 0, 150, 255, 200)
        Color lightBlue = Color.FromArgb(200, 0, 210, 255);

        // Draw LSIA marker at the original location
        World.DrawMarker(
            MarkerType.VerticalCylinder,
            lsiaBlipLocation,
            Vector3.Zero,
            new Vector3(0f, 0f, -1f), // Offset slightly below the marker point
            new Vector3(0.8f, 0.8f, 1.1f), //radius
            lightBlue // Use light blue color
        );

        // Draw Cayo Perico marker at the original location
        World.DrawMarker(
            MarkerType.VerticalCylinder,
            cayoBlipLocation,
            Vector3.Zero,
            new Vector3(0f, 0f, -1f), // Offset slightly below the marker point
            new Vector3(0.8f, 0.8f, 1.1f), //radius
            lightBlue // Use light blue color
        );
        //LSIA -> north yankton, marker
        World.DrawMarker(
            MarkerType.VerticalCylinder,
            NYlsiaBlip,
            Vector3.Zero,
            new Vector3(0f, 0f, -1f), // Offset slightly below the marker point
            new Vector3(0.8f, 0.8f, 1.1f), //radius
            lightBlue // Use light blue color
        );
        //North yankton marker
        World.DrawMarker(
            MarkerType.VerticalCylinder,
            NY_BlipLocation,
            Vector3.Zero,
            new Vector3(0f, 0f, -1f), // Offset slightly below the marker point
            new Vector3(0.8f, 0.8f, 1.1f), //radius
            lightBlue // Use light blue color
        );
    }

    private void OnTick(object sender, EventArgs e)
    {
        // Draw markers continuously
        DrawMarkers();

        // Perform proximity check for Cayo Perico
        if (isCayoProximityEnabled)
        {
            CayoPericoProximity();
        }
        // Check if player is near LSIA location
        if (Game.Player.Character.Position.DistanceTo(lsiaBlipLocation) < 1f)
        {
            Notification.Show("Press ~o~E~s~ to fly to Cayo Perico");
        }
        // Check if player is near Cayo Perico location
        else if (Game.Player.Character.Position.DistanceTo(cayoBlipLocation) < 1f)
        {
            Notification.Show("Press ~o~E~s~ to return to Los Santos");
        }
        // Check if player is near North Yankton location
        else if (Game.Player.Character.Position.DistanceTo(NYlsiaBlip) < 1f)
        {
            Notification.Show("Press ~o~E~s~ to fly to North Yankton");
        }
        // Check if player is at North Yankton and can return to LSIA
        else if (Game.Player.Character.Position.DistanceTo(NY_BlipLocation) < 1f)
        {
            Notification.Show("Press ~o~E~s~ to return to Los Santos");
        }

        // Update Cayo Perico blip visibility
        cayoBlip.Alpha = isCayoPericoEnabled ? 255 : 0;

        // Update North Yankton blip visibility
        NorthYBlip.Alpha = LoadNY.isLoaded ? 255 : 0;

        // Update LSIA blip visibility
        lsiaBlip.Alpha = (!isCayoPericoEnabled && !LoadNY.isLoaded) ? 255 : 0;

        // Update LSIA -> North Yankton blip visibility
        NorthYlsiaBlip.Alpha = (LoadNY.isLoaded || isCayoPericoEnabled) ? 0 : 255; // || means "or"

        //check if player is below Z: 30 each tick
        NorthYanktonPositionCheck();
    }



    private void CayoPericoProximity()
    {
        float distanceToCayo = Game.Player.Character.Position.DistanceTo(cayoBlipLocation);
        if (distanceToCayo < 1800.0f && !isCayoPericoEnabled && !isFading)
        {
            FadeScreenAndHandleCayoPerico(true);
        }
        else if (distanceToCayo >= 1800.0f && isCayoPericoEnabled && !isFading)
        {
            FadeScreenAndHandleCayoPerico(false);
        }
    }




    //***********************************************************************************************************
    //************************   markers destinations   *********************************************************
    //***********************************************************************************************************

    //! this pattern is followed: bool toCayoPerico, bool toNY, bool isManualTeleport, bool toNYlsia = false
    //bool can be either true or false
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        // Check if player is at LSIA and pressed "E" to teleport to Cayo Perico
        if (e.KeyCode == Keys.E && Game.Player.Character.Position.DistanceTo(lsiaBlipLocation) < 1.5f)
        {
            if (!isCayoPericoEnabled)
            {
                FadeScreenAndTeleport(true, false, true); // Teleport to Cayo Perico
            }
        }
        // Check if player is at Cayo Perico and pressed "E" to teleport back to Los Santos
        else if (e.KeyCode == Keys.E && Game.Player.Character.Position.DistanceTo(cayoBlipLocation) < 1.5f)
        {
            if (isCayoPericoEnabled)
            {
                FadeScreenAndTeleport(false, false, true); // Teleport to Los Santos from Cayo Perico
            }
        }
        // Check if player is at LSIA for North Yankton and pressed "E" to teleport to North Yankton
        else if (e.KeyCode == Keys.E && Game.Player.Character.Position.DistanceTo(NYlsiaBlip) < 1.5f)
        {
            FadeScreenAndTeleport(false, true, true); // Teleport to North Yankton
        }
        else if (e.KeyCode == Keys.E && Game.Player.Character.Position.DistanceTo(NY_BlipLocation) < 1.5f)
        {
            FadeScreenAndTeleport(false, false, true, true); // Teleport to LSIA from North Yankton
        }
    }

    //***********************************************************************************************************
    //************************   hashes for cayo   **************************************************************
    //***********************************************************************************************************
    private void EnableCayoPerico(bool enable)
    {
        if (enable)
        {
            Function.Call((Hash)0x9A9D1BA639675CF1, "HeistIsland", true, false); // Load Cayo Perico Island
            Function.Call((Hash)0x5E1460624D194A38, true); // Load the minimap and map
            Function.Call((Hash)0xF8DEE0A5600CBB93, true);  //this reveals the grayed/unexplored map parts in story mode, without having to explore it, since cayo map is bugged in story mode, this has to be used.

            // Enable scenarios and NPC spawning
            Function.Call(Hash.SET_SCENARIO_GROUP_ENABLED, "Heist_Island_Peds", true);
            Function.Call((Hash)0x9A9D1BA639675CF1, "HeistIsland", true); // set island hopper enabled
            Function.Call((Hash)0xF74B1FFA4A15FBEA, 1); // enable path nodes so routing works on island
            //enable zone
            int zoneId = Function.Call<int>(Hash.GET_ZONE_FROM_NAME_ID, "IsHeistZone");
            Function.Call(Hash.SET_ZONE_ENABLED, zoneId, true);



            // Radar configuration
            if (Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Game.Player.Character) == 0)
            {
                Function.Call(Hash.SET_RADAR_AS_EXTERIOR_THIS_FRAME);
                Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, 0xc0a90510, 4700.0f, -5145.0f, 0, 0);
            }

            isCayoPericoEnabled = true;
        }
        else
        {
            // Disable scenarios and NPC spawning
            Function.Call(Hash.SET_SCENARIO_GROUP_ENABLED, "Heist_Island_Peds", false);
            Function.Call((Hash)0x5E1460624D194A38, false);
            Function.Call((Hash)0x9A9D1BA639675CF1, "HeistIsland", false);
            Function.Call((Hash)0xF74B1FFA4A15FBEA, 0); // disable path nodes so routing works on mainland
            int zoneId = Function.Call<int>(Hash.GET_ZONE_FROM_NAME_ID, "IsHeistZone");
            Function.Call(Hash.SET_ZONE_ENABLED, zoneId, true);
            isCayoPericoEnabled = false;
        }
    }

    //***********************************************************************************************************
    //************************   screen fades   *****************************************************************
    //***********************************************************************************************************

    private void FadeScreenAndTeleport(bool toCayoPerico, bool toNY, bool isManualTeleport, bool toNYlsia = false)
    {
        isFading = true;

        // Fade out the screen
        Function.Call(Hash.DO_SCREEN_FADE_OUT, 600);
        Wait(100);

        // Determine the teleport location based on destination
        Vector3 teleportLocation = Vector3.Zero; // Initialize teleportLocation

        if (toCayoPerico)
        {
            EnableCayoPerico(true);
            teleportLocation = cayoTeleportLocation;
        }
        else if (toNY)
        {
            LoadNY.RequestNY(this);
            teleportLocation = NY_TeleportLocation;
        }
        else if (toNYlsia)
        {
            EnableCayoPerico(false);
            LoadNY.UnloadNY(this);
            teleportLocation = NYlsiaTeleportLocation;
        }
        else
        {
            // fallback, if nothing is met
            EnableCayoPerico(false); // Ensure Cayo Perico is disabled when returning to LSIA
            teleportLocation = lsiaTeleportLocation; // default/fallback
        }

        Wait(700);

        // Teleport the player
        Game.Player.Character.Position = teleportLocation;

        // Fade in the screen
        Function.Call(Hash.DO_SCREEN_FADE_IN, 600);

        // If this was an automatic teleport, reset isFading after a short delay
        if (!isManualTeleport)
        {
            Wait(700);
        }

        isFading = false;
    }



    private void FadeScreenAndHandleCayoPerico(bool enableCayoPerico)
    {
        isFading = true;

        // Fade out the screen
        Function.Call(Hash.DO_SCREEN_FADE_OUT, 600);

        Wait(1000);
        EnableCayoPerico(enableCayoPerico);
        // Fade in the screen
        Function.Call(Hash.DO_SCREEN_FADE_IN, 600);
        // Reset isFading after a short delay
        Wait(700);

        isFading = false;
    }

    private void FadeScreenAndHandleNY(bool enableNY)
    {
        isFading = true;

        // Fade out the screen
        Function.Call(Hash.DO_SCREEN_FADE_OUT, 600);

        Wait(1000);
        // Fade in the screen
        Function.Call(Hash.DO_SCREEN_FADE_IN, 600);
        Wait(700);

        // Reset isFading after a short delay
        Wait(700);

        isFading = false;
        if (enableNY)
        {
            LoadNY.RequestNY(this);
            Game.Player.Character.Position = NY_TeleportLocation; // Teleport to NY
        }
        else
        {
            LoadNY.UnloadNY(this);
            Game.Player.Character.Position = NYlsiaTeleportLocation; // Teleport to LS
        }
    }
}