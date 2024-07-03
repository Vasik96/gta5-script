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
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics;


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
    public bool isCayoProximityEnabled = true; 

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
        LoadCayoIPLs();


        // Show startup message
        GTA.UI.Screen.ShowHelpText("Enabled more locations, enjoy.");



    }


    private void LoadCayoIPLs()
    {
        string[] iplNames = new string[]
                {
                "h4_mph4_terrain_01_grass_0",
                "h4_mph4_terrain_01_grass_1",
                "h4_mph4_terrain_02_grass_0",
                "h4_mph4_terrain_02_grass_1",
                "h4_mph4_terrain_02_grass_2",
                "h4_mph4_terrain_02_grass_3",
                "h4_mph4_terrain_04_grass_0",
                "h4_mph4_terrain_04_grass_1",
                "h4_mph4_terrain_05_grass_0",
                "h4_mph4_terrain_06_grass_0",
                "h4_islandx_terrain_01",
                "h4_islandx_terrain_01_lod",
                "h4_islandx_terrain_01_slod",
                "h4_islandx_terrain_02",
                "h4_islandx_terrain_02_lod",
                "h4_islandx_terrain_02_slod",
                "h4_islandx_terrain_03",
                "h4_islandx_terrain_03_lod",
                "h4_islandx_terrain_04",
                "h4_islandx_terrain_04_lod",
                "h4_islandx_terrain_04_slod",
                "h4_islandx_terrain_05",
                "h4_islandx_terrain_05_lod",
                "h4_islandx_terrain_05_slod",
                "h4_islandx_terrain_06",
                "h4_islandx_terrain_06_lod",
                "h4_islandx_terrain_06_slod",
                "h4_islandx_terrain_props_05_a",
                "h4_islandx_terrain_props_05_a_lod",
                "h4_islandx_terrain_props_05_b",
                "h4_islandx_terrain_props_05_b_lod",
                "h4_islandx_terrain_props_05_c",
                "h4_islandx_terrain_props_05_c_lod",
                "h4_islandx_terrain_props_05_d",
                "h4_islandx_terrain_props_05_d_lod",
                "h4_islandx_terrain_props_05_d_slod",
                "h4_islandx_terrain_props_05_e",
                "h4_islandx_terrain_props_05_e_lod",
                "h4_islandx_terrain_props_05_e_slod",
                "h4_islandx_terrain_props_05_f",
                "h4_islandx_terrain_props_05_f_lod",
                "h4_islandx_terrain_props_05_f_slod",
                "h4_islandx_terrain_props_06_a",
                "h4_islandx_terrain_props_06_a_lod",
                "h4_islandx_terrain_props_06_a_slod",
                "h4_islandx_terrain_props_06_b",
                "h4_islandx_terrain_props_06_b_lod",
                "h4_islandx_terrain_props_06_b_slod",
                "h4_islandx_terrain_props_06_c",
                "h4_islandx_terrain_props_06_c_lod",
                "h4_islandx_terrain_props_06_c_slod",
                "h4_mph4_terrain_01",
                "h4_mph4_terrain_01_long_0",
                "h4_mph4_terrain_02",
                "h4_mph4_terrain_03",
                "h4_mph4_terrain_04",
                "h4_mph4_terrain_05",
                "h4_mph4_terrain_06",
                "h4_mph4_terrain_06_strm_0",
                "h4_mph4_terrain_lod",
                "h4_mph4_terrain_occ_00",
                "h4_mph4_terrain_occ_01",
                "h4_mph4_terrain_occ_02",
                "h4_mph4_terrain_occ_03",
                "h4_mph4_terrain_occ_04",
                "h4_mph4_terrain_occ_05",
                "h4_mph4_terrain_occ_06",
                "h4_mph4_terrain_occ_07",
                "h4_mph4_terrain_occ_08",
                "h4_mph4_terrain_occ_09",
                "h4_boatblockers",
                "h4_islandx",
                "h4_islandx_disc_strandedshark",
                "h4_islandx_disc_strandedshark_lod",
                "h4_islandx_disc_strandedwhale",
                "h4_islandx_disc_strandedwhale_lod",
                "h4_islandx_props",
                "h4_islandx_props_lod",
                "h4_islandx_sea_mines",
                "h4_mph4_island",
                "h4_mph4_island_long_0",
                "h4_mph4_island_strm_0",
                "h4_aa_guns",
                "h4_aa_guns_lod",
                "h4_beach",
                "h4_beach_bar_props",
                "h4_beach_lod",
                "h4_beach_party",
                "h4_beach_party_lod",
                "h4_beach_props",
                "h4_beach_props_lod",
                "h4_beach_props_party",
                "h4_beach_props_slod",
                "h4_beach_slod",
                "h4_islandairstrip",
                "h4_islandairstrip_doorsclosed",
                "h4_islandairstrip_doorsclosed_lod",
                "h4_islandairstrip_hangar_props",
                "h4_islandairstrip_hangar_props_lod",
                "h4_islandairstrip_hangar_props_slod",
                "h4_islandairstrip_lod",
                "h4_islandairstrip_props",
                "h4_islandairstrip_propsb",
                "h4_islandairstrip_propsb_lod",
                "h4_islandairstrip_propsb_slod",
                "h4_islandairstrip_props_lod",
                "h4_islandairstrip_props_slod",
                "h4_islandairstrip_slod",
                "h4_islandxcanal_props",
                "h4_islandxcanal_props_lod",
                "h4_islandxcanal_props_slod",
                "h4_islandxdock",
                "h4_islandxdock_lod",
                "h4_islandxdock_props",
                "h4_islandxdock_props_2",
                "h4_islandxdock_props_2_lod",
                "h4_islandxdock_props_2_slod",
                "h4_islandxdock_props_lod",
                "h4_islandxdock_props_slod",
                "h4_islandxdock_slod",
                "h4_islandxdock_water_hatch",
                "h4_islandxtower",
                "h4_islandxtower_lod",
                "h4_islandxtower_slod",
                "h4_islandxtower_veg",
                "h4_islandxtower_veg_lod",
                "h4_islandxtower_veg_slod",
                "h4_islandx_barrack_hatch",
                "h4_islandx_barrack_props",
                "h4_islandx_barrack_props_lod",
                "h4_islandx_barrack_props_slod",
                "h4_islandx_checkpoint",
                "h4_islandx_checkpoint_lod",
                "h4_islandx_checkpoint_props",
                "h4_islandx_checkpoint_props_lod",
                "h4_islandx_checkpoint_props_slod",
                "h4_islandx_maindock",
                "h4_islandx_maindock_lod",
                "h4_islandx_maindock_props",
                "h4_islandx_maindock_props_2",
                "h4_islandx_maindock_props_2_lod",
                "h4_islandx_maindock_props_2_slod",
                "h4_islandx_maindock_props_lod",
                "h4_islandx_maindock_props_slod",
                "h4_islandx_maindock_slod",
                "h4_islandx_mansion",
                "h4_islandx_mansion_b",
                "h4_islandx_mansion_b_lod",
                "h4_islandx_mansion_b_side_fence",
                "h4_islandx_mansion_b_slod",
                "h4_islandx_mansion_entrance_fence",
                "h4_islandx_mansion_guardfence",
                "h4_islandx_mansion_lights",
                "h4_islandx_mansion_lockup_01",
                "h4_islandx_mansion_lockup_01_lod",
                "h4_islandx_mansion_lockup_02",
                "h4_islandx_mansion_lockup_02_lod",
                "h4_islandx_mansion_lockup_03",
                "h4_islandx_mansion_lockup_03_lod",
                "h4_islandx_mansion_lod",
                "h4_islandx_mansion_office",
                "h4_islandx_mansion_office_lod",
                "h4_islandx_mansion_props",
                "h4_islandx_mansion_props_lod",
                "h4_islandx_mansion_props_slod",
                "h4_islandx_mansion_slod",
                "h4_islandx_mansion_vault",
                "h4_islandx_mansion_vault_lod",
                "h4_island_padlock_props",
                "h4_mansion_gate_closed",
                "h4_mansion_remains_cage",
                "h4_mph4_airstrip",
                "h4_mph4_airstrip_interior_0_airstrip_hanger",
                "h4_mph4_beach",
                //"h4_airstrip_hanger", //not IPL, it is the interior
                "h4_mph4_dock",
                "h4_mph4_island_lod",
                "h4_mph4_island_ne_placement",
                "h4_mph4_island_nw_placement",
                "h4_mph4_island_se_placement",
                "h4_mph4_island_sw_placement",
                "h4_mph4_mansion",
                "h4_mph4_mansion_b",
                "h4_mph4_mansion_b_strm_0",
                "h4_mph4_mansion_strm_0",
                "h4_mph4_wtowers",
                "h4_ne_ipl_00",
                "h4_ne_ipl_00_lod",
                "h4_ne_ipl_00_slod",
                "h4_ne_ipl_01",
                "h4_ne_ipl_01_lod",
                "h4_ne_ipl_01_slod",
                "h4_ne_ipl_02",
                "h4_ne_ipl_02_lod",
                "h4_ne_ipl_02_slod",
                "h4_ne_ipl_03",
                "h4_ne_ipl_03_lod",
                "h4_ne_ipl_03_slod",
                "h4_ne_ipl_04",
                "h4_ne_ipl_04_lod",
                "h4_ne_ipl_04_slod",
                "h4_ne_ipl_05",
                "h4_ne_ipl_05_lod",
                "h4_ne_ipl_05_slod",
                "h4_ne_ipl_06",
                "h4_ne_ipl_06_lod",
                "h4_ne_ipl_06_slod",
                "h4_ne_ipl_07",
                "h4_ne_ipl_07_lod",
                "h4_ne_ipl_07_slod",
                "h4_ne_ipl_08",
                "h4_ne_ipl_08_lod",
                "h4_ne_ipl_08_slod",
                "h4_ne_ipl_09",
                "h4_ne_ipl_09_lod",
                "h4_ne_ipl_09_slod",
                "h4_nw_ipl_00",
                "h4_nw_ipl_00_lod",
                "h4_nw_ipl_00_slod",
                "h4_nw_ipl_01",
                "h4_nw_ipl_01_lod",
                "h4_nw_ipl_01_slod",
                "h4_nw_ipl_02",
                "h4_nw_ipl_02_lod",
                "h4_nw_ipl_02_slod",
                "h4_nw_ipl_03",
                "h4_nw_ipl_03_lod",
                "h4_nw_ipl_03_slod",
                "h4_nw_ipl_04",
                "h4_nw_ipl_04_lod",
                "h4_nw_ipl_04_slod",
                "h4_nw_ipl_05",
                "h4_nw_ipl_05_lod",
                "h4_nw_ipl_05_slod",
                "h4_nw_ipl_06",
                "h4_nw_ipl_06_lod",
                "h4_nw_ipl_06_slod",
                "h4_nw_ipl_07",
                "h4_nw_ipl_07_lod",
                "h4_nw_ipl_07_slod",
                "h4_nw_ipl_08",
                "h4_nw_ipl_08_lod",
                "h4_nw_ipl_08_slod",
                "h4_nw_ipl_09",
                "h4_nw_ipl_09_lod",
                "h4_nw_ipl_09_slod",
                "h4_se_ipl_00",
                "h4_se_ipl_00_lod",
                "h4_se_ipl_00_slod",
                "h4_se_ipl_01",
                "h4_se_ipl_01_lod",
                "h4_se_ipl_01_slod",
                "h4_se_ipl_02",
                "h4_se_ipl_02_lod",
                "h4_se_ipl_02_slod",
                "h4_se_ipl_03",
                "h4_se_ipl_03_lod",
                "h4_se_ipl_03_slod",
                "h4_se_ipl_04",
                "h4_se_ipl_04_lod",
                "h4_se_ipl_04_slod",
                "h4_se_ipl_05",
                "h4_se_ipl_05_lod",
                "h4_se_ipl_05_slod",
                "h4_se_ipl_06",
                "h4_se_ipl_06_lod",
                "h4_se_ipl_06_slod",
                "h4_se_ipl_07",
                "h4_se_ipl_07_lod",
                "h4_se_ipl_07_slod",
                "h4_se_ipl_08",
                "h4_se_ipl_08_lod",
                "h4_se_ipl_08_slod",
                "h4_se_ipl_09",
                "h4_se_ipl_09_lod",
                "h4_se_ipl_09_slod",
                "h4_sw_ipl_00",
                "h4_sw_ipl_00_lod",
                "h4_sw_ipl_00_slod",
                "h4_sw_ipl_01",
                "h4_sw_ipl_01_lod",
                "h4_sw_ipl_01_slod",
                "h4_sw_ipl_02",
                "h4_sw_ipl_02_lod",
                "h4_sw_ipl_02_slod",
                "h4_sw_ipl_03",
                "h4_sw_ipl_03_lod",
                "h4_sw_ipl_03_slod",
                "h4_sw_ipl_04",
                "h4_sw_ipl_04_lod",
                "h4_sw_ipl_04_slod",
                "h4_sw_ipl_05",
                "h4_sw_ipl_05_lod",
                "h4_sw_ipl_05_slod",
                "h4_sw_ipl_06",
                "h4_sw_ipl_06_lod",
                "h4_sw_ipl_06_slod",
                "h4_sw_ipl_07",
                "h4_sw_ipl_07_lod",
                "h4_sw_ipl_07_slod",
                "h4_sw_ipl_08",
                "h4_sw_ipl_08_lod",
                "h4_sw_ipl_08_slod",
                "h4_sw_ipl_09",
                "h4_sw_ipl_09_lod",
                "h4_sw_ipl_09_slod",
                "h4_underwater_gate_closed",
                "h4_islandx_placement_01",
                "h4_islandx_placement_02",
                "h4_islandx_placement_03",
                "h4_islandx_placement_04",
                "h4_islandx_placement_05",
                "h4_islandx_placement_06",
                "h4_islandx_placement_07",
                "h4_islandx_placement_08",
                "h4_islandx_placement_09",
                "h4_islandx_placement_10",
                "h4_mph4_island_placement",
                "h4_int_placement_h4_interior_1_dlc_int_02_h4_milo_"

                };

        // Loop through each IPL name and request it
        foreach (string iplName in iplNames)
        {
            Function.Call<bool>(Hash.REQUEST_IPL, iplName);
        }
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
                //military base aircraft carrier - dlc: mp 2024_01 (summer)
                "m24_1_carrier",
                "m24_1_carrier_int1",
                "m24_1_carrier_int2",
                "m24_1_carrier_int3",
                "m24_1_carrier_int4",
                "m24_1_carrier_int5",
                "m24_1_carrier_int6",
                "m24_1_carrier_ladders",
                "m24_1_legacyfixes",
                "m24_1_pizzasigns",
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


    private void SetRandomWeatherForNY()
    {
        Random random = new Random();
        int weatherIndex = random.Next(100); // Generate a number between 0 and 99

        if (weatherIndex < 38)
        {
            // 38% chance for "SNOWLIGHT"
            Function.Call(Hash.SET_OVERRIDE_WEATHER, "SNOWLIGHT");
        }
        else if (weatherIndex < 69)
        {
            // 31% chance for "SNOW"
            Function.Call(Hash.SET_OVERRIDE_WEATHER, "SNOW");
        }
        else
        {
            // 31% chance for "BLIZZARD"
            Function.Call(Hash.SET_OVERRIDE_WEATHER, "BLIZZARD");
        }
    }


    private void ClearOverrideWeather()
    {
        Function.Call(Hash.SET_OVERRIDE_WEATHER, "CLEAR");
        Function.Call(Hash.CLEAR_OVERRIDE_WEATHER);
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
            "prologue03_grv_dug",
            "prologue03_grv_dug_lod",
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
                    Function.Call(Hash.SET_ALLOW_STREAM_PROLOGUE_NODES, true); //nodes for the location - confirmed it works, hash: 0x228E5C6AD4D74BFD


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
            "prologue03_grv_dug",
            "prologue03_grv_dug_lod",
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

                    int zoneId = Function.Call<int>(Hash.GET_ZONE_FROM_NAME_ID, "PrLog");
                    Function.Call(Hash.SET_ZONE_ENABLED, zoneId, false);

                    Function.Call(Hash.SET_MAPDATACULLBOX_ENABLED, "prologue", false);
                    Function.Call(Hash.SET_MAPDATACULLBOX_ENABLED, "Prologue_Main", false); //idk what these do, but they are related to NY, so ill keep them here
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 5655.24f, -5142.23f, 61.78925f, 3679.327f, -4973.879f, 125.0828f, 192, false, false, false);
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 3691.211f, -4941.24f, 94.59368f, 3511.115f, -4869.191f, 126.7621f, 16, false, false, false);
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 3510.004f, -4865.81f, 94.69557f, 3204.424f, -4833.817f, 126.8152f, 16, false, false, false);
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 3186.534f, -4832.798f, 109.8148f, 3202.187f, -4833.993f, 114.815f, 16, false, false, false);
                    //1 side road had to be added manually
                    Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 5493.3f, -5344.76f, 81.8f, 5483.187f, -5137.3f, 75.1f, 4, false, false, false);
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
        lsiaBlip.Sprite = BlipSprite.CayoPericoSeries;
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
        if (Game.Player.Character.Position.DistanceTo(lsiaBlipLocation) < 1.5f)
        {
            GTA.UI.Screen.ShowHelpText("Press ~INPUT_CONTEXT~ to fly to ~y~Cayo Perico~s~");
        }
        // Check if player is near Cayo Perico location
        else if (Game.Player.Character.Position.DistanceTo(cayoBlipLocation) < 1.5f)
        {
            GTA.UI.Screen.ShowHelpText("Press ~INPUT_CONTEXT~ to return to Los Santos");
        }
        // Check if player is near North Yankton location
        else if (Game.Player.Character.Position.DistanceTo(NYlsiaBlip) < 1.5f)
        {
            GTA.UI.Screen.ShowHelpText("Press ~INPUT_CONTEXT~ to fly to ~b~North Yankton~s~");
        }
        // Check if player is at North Yankton and can return to LSIA
        else if (Game.Player.Character.Position.DistanceTo(NY_BlipLocation) < 1.5f)
        {
            GTA.UI.Screen.ShowHelpText("Press ~INPUT_CONTEXT~ to return to Los Santos");
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

    private Vector2 cayoCenter = new Vector2(4990f, -5100f);

    private void CayoPericoProximity()
    {
        Vector3 playerPosition = Game.Player.Character.Position;
        Vector2 playerPosition2D = new Vector2(playerPosition.X, playerPosition.Y);

        float distanceToCayo = Vector2.Distance(playerPosition2D, cayoCenter);

        if (distanceToCayo < 2000.0f && !isCayoPericoEnabled && !isFading)
        {
            manuallyTravelling = true;
            EnableCayo(true);
        }
        else if (distanceToCayo >= 2000.0f && isCayoPericoEnabled && !isFading)
        {
            manuallyTravelling = false;
            EnableCayo(false);
        }
    }


    private void EnableCayo(bool enableCayoPerico)
    {
        //Function.Call(Hash.DO_SCREEN_FADE_OUT, 700);
        Wait(1000);
        EnableCayoPerico(enableCayoPerico);
        Wait(1000);
        //Function.Call(Hash.DO_SCREEN_FADE_IN, 700);
    }


    


    //***********************************************************************************************************
    //************************   markers destinations   *********************************************************
    //***********************************************************************************************************

    //! this pattern is followed: bool toCayoPerico, bool toNY, bool isManualTeleport, bool toNYlsia = false
    //bool can be either true or false


    public bool isF5Pressed = false;
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        // Check if player is at LSIA and pressed "E" to teleport to Cayo Perico
        if (e.KeyCode == Keys.E && Game.Player.Character.Position.DistanceTo(lsiaBlipLocation) < 1.5f)
        {
            if (!isCayoPericoEnabled)
            {
                HandleLocations(true, false, true); // Teleport to Cayo Perico
            }
        }
        // Check if player is at Cayo Perico and pressed "E" to teleport back to Los Santos
        else if (e.KeyCode == Keys.E && Game.Player.Character.Position.DistanceTo(cayoBlipLocation) < 1.5f)
        {
            if (isCayoPericoEnabled)
            {
                HandleLocations(false, false, true); // Teleport to Los Santos from Cayo Perico
                manuallyTravelling = false;
                CayoTime();
            }
        }
        // Check if player is at LSIA for North Yankton and pressed "E" to teleport to North Yankton
        else if (e.KeyCode == Keys.E && Game.Player.Character.Position.DistanceTo(NYlsiaBlip) < 1.5f)
        {
            HandleLocations(false, true, true); // Teleport to North Yankton
        }
        else if (e.KeyCode == Keys.E && Game.Player.Character.Position.DistanceTo(NY_BlipLocation) < 1.5f)
        {
            HandleLocations(false, false, true, true); // Teleport to LSIA from North Yankton
        }
        else if (e.KeyCode == Keys.F5)
        {
            Function.Call(Hash.TRIGGER_MUSIC_EVENT, "MIC1_TREVOR_PLANE");
        }
        else if (e.KeyCode == Keys.F6)
        {
            Function.Call(Hash.CANCEL_MUSIC_EVENT, "MIC1_TREVOR_PLANE");
        }
    }


    //***********************************************************************************************************
    //************************   hashes for cayo   **************************************************************
    //***********************************************************************************************************


    private void EnableCayoPerico(bool enable)
    {
        if (enable)
        {
            
            // Load Cayo Perico Island
            //Function.Call((Hash)0x9A9D1BA639675CF1, "HeistIsland", true, false); // Load Cayo Perico Island, islandhopper func.
            Function.Call((Hash)0x7E3F55ED251B76D3, 1); //1 - heistisland, 0 - default

            // Disable Yankton zone before loading Cayo Perico
            int yanktonZoneId = Function.Call<int>(Hash.GET_ZONE_FROM_NAME_ID, "PrLog");
            Function.Call(Hash.SET_ZONE_ENABLED, yanktonZoneId, false);
            
            //disable arena stuff
            Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "SE_DLC_AW_ARENA_CONSTRUCTION_01", false);
            Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "SE_DLC_AW_ARENA_CROWD_BACKGROUND_MAIN", false);
            Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "SE_DLC_AW_CROWD_EXTERIOR_LOBBY", false);
            Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "SE_DLC_AW_CROWD_INTERIOR_LOBBY", false);


            //string cayoZoneName = Function.Call<string>(Hash.GET_NAME_OF_ZONE, 4840.571f, -5174.425f, 2.0f);
            //int cayoZone = Function.Call<int>(Hash.GET_ZONE_FROM_NAME_ID, cayoZoneName);
            //Function.Call(Hash.SET_ZONE_ENABLED, cayoZoneName, true);

            // Enable minimap and reveal unexplored parts of the map
            Function.Call((Hash)0x5E1460624D194A38, true); // Load the minimap and map
            Function.Call(Hash.SET_AMBIENT_ZONE_LIST_STATE_PERSISTENT, "AZL_DLC_Hei4_Island_Disabled_Zones", 0, 1);
            Function.Call(Hash.SET_AMBIENT_ZONE_LIST_STATE_PERSISTENT, "AZL_DLC_Hei4_Island_Zones", 1, 1);
            //Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_dlc_hei4_island_beach_party_music_new_01_left", true);
            //Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_dlc_hei4_island_beach_party_music_new_02_right", true);
            /*int zoneId = Function.Call<int>(Hash.GET_ZONE_FROM_NAME_ID, "IsHeistZone");
            Function.Call(Hash.SET_ZONE_ENABLED, zoneId, true);*/
            Function.Call((Hash)0xF8DEE0A5600CBB93, true);  // Reveal the grayed/unexplored map parts in story mode

            // Enable scenarios and NPC spawning
            Function.Call(Hash.SET_SCENARIO_GROUP_ENABLED, "Heist_Island_Peds", true);
            Function.Call((Hash)0x53797676AD34A9AA, true); // unknown
            Function.Call((Hash)0xF74B1FFA4A15FBEA, 1); // Enable path nodes so routing works on the island
            LoadCayoIPLs(); //load


            //interior
            Function.Call(Hash.REQUEST_IPL, "h4_mph4_airstrip_interior_0_airstrip_hanger");
            while (!Function.Call<bool>(Hash.IS_IPL_ACTIVE, "h4_mph4_airstrip_interior_0_airstrip_hanger"))
            {
                Script.Wait(0);
                GTA.UI.Screen.ShowHelpText("a");
            }
            Function.Call(Hash.DISABLE_INTERIOR, 1104, false);
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, 1104, "island_hanger_padlock_props"); //interior ID: 1104, 739th interior
            Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, 1104);
            Function.Call(Hash.REFRESH_INTERIOR, 1104);
            


            // Audio
            //Function.Call(Hash.SET_AUDIO_FLAG, "PlayerOnDLCHeist4Island", 1);

            //Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 6371.534f, -5965.798f, 200f, 3421.187f, -4374.993f, 0f, 384, false, true, true);

            // Radar configuration
            if (Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Game.Player.Character) == 0)
            {
                Function.Call(Hash.SET_RADAR_AS_EXTERIOR_THIS_FRAME);
                Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, "h4_fake_islandx", 4700.0f, -5145.0f, 0f, 0);
            }

            isCayoPericoEnabled = true;
            Wait(2);
            CayoTime();
        }
        else
        {
            //Function.Call((Hash)
            //, "HeistIsland", false); // Disable the Cayo Perico Island, island hopper func.
            Function.Call((Hash)0x7E3F55ED251B76D3, 0); //1 - heistisland, 0 - default, LOAD_GLOBAL_WATER_TYPE func.

            // Disable scenarios and NPC spawning
            Function.Call(Hash.SET_SCENARIO_GROUP_ENABLED, "Heist_Island_Peds", false); 
            Function.Call((Hash)0x5E1460624D194A38, false); //minimap, map
            
            Function.Call((Hash)0x53797676AD34A9AA, false); // unknown


            Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "SE_DLC_AW_ARENA_CONSTRUCTION_01", true);
            Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "SE_DLC_AW_ARENA_CROWD_BACKGROUND_MAIN", true);
            Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "SE_DLC_AW_CROWD_EXTERIOR_LOBBY", true);
            Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "SE_DLC_AW_CROWD_INTERIOR_LOBBY", true);

            // Disable the roads in the area
            //Function.Call(Hash.SET_ROADS_IN_ANGLED_AREA, 6371.534f, -5965.798f, 200f, 3421.187f, -4374.993f, 0f, 384, false, false, false);

            Function.Call((Hash)0xF74B1FFA4A15FBEA, 0); // Disable path nodes so routing works on mainland

            //string cayoZoneName = Function.Call<string>(Hash.GET_NAME_OF_ZONE, 4840.571f, -5174.425f, 2.0f); // Example coordinates within Cayo Perico
            //int cayoZone = Function.Call<int>(Hash.GET_ZONE_FROM_NAME_ID, cayoZoneName);
            //Function.Call(Hash.SET_ZONE_ENABLED, cayoZone, false);

            // Audio
            //Function.Call(Hash.SET_AUDIO_FLAG, "PlayerOnDLCHeist4Island", 0);

            // Disable Cayo Perico zone
            /*int zoneId = Function.Call<int>(Hash.GET_ZONE_FROM_NAME_ID, "IsHeistZone");
            Function.Call(Hash.SET_ZONE_ENABLED, zoneId, false);*/
            Function.Call(Hash.SET_AMBIENT_ZONE_LIST_STATE_PERSISTENT, "AZL_DLC_Hei4_Island_Zones", 0, 0);
            Function.Call(Hash.SET_AMBIENT_ZONE_LIST_STATE_PERSISTENT, "AZL_DLC_Hei4_Island_Disabled_Zones", 1, 0);
            //Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_dlc_hei4_island_beach_party_music_new_01_left", false);
            //Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_dlc_hei4_island_beach_party_music_new_02_right", false);

            isCayoPericoEnabled = false;
            Wait(2);
            CayoTime();
            Wait(5);
            LoadCayoIPLs();
        }
    }


    private bool isCayoTimeSwitched = false;
    private int previousHour = 0;
    private int previousMinute = 0;
    bool manuallyTravelling = false;

    private void CayoTime()
    {

        if (manuallyTravelling == false)
        {


            if (isCayoPericoEnabled && !isCayoTimeSwitched)
            {
                previousHour = Function.Call<int>(Hash.GET_CLOCK_HOURS);
                previousMinute = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
                int newHour = (previousHour + 12) % 24;
                Function.Call(Hash.SET_CLOCK_TIME, newHour, previousMinute, 0);
                isCayoTimeSwitched = true;
            }
            else if (!isCayoPericoEnabled && isCayoTimeSwitched)
            {
                int currentHour = Function.Call<int>(Hash.GET_CLOCK_HOURS);
                int revertHour = (currentHour - 12 + 24) % 24; // Ensure we wrap around correctly
                Function.Call(Hash.SET_CLOCK_TIME, revertHour, previousMinute, 0);
                isCayoTimeSwitched = false;
            }
        }
    }

    //***********************************************************************************************************
    //************************   screen fades   *****************************************************************
    //***********************************************************************************************************

    private void HandleLocations(bool toCayoPerico, bool toNY, bool manuallyTravelled, bool toNYlsia = false)
    {
        isFading = true;

        // Fade out the screen
        Function.Call(Hash.DO_SCREEN_FADE_OUT, 700);
        Wait(500);

        // Determine the teleport location based on destination
        Vector3 teleportLocation = Vector3.Zero; // Initialize teleportLocation

        if (toCayoPerico)
        {
            EnableCayoPerico(true);
            Wait(500);
            teleportLocation = cayoTeleportLocation;
            Wait(500);
        }
        else if (toNY)
        {
            Wait(300);
            LoadNY.RequestNY(this);
            Wait(300);
            teleportLocation = NY_TeleportLocation;
        }
        else if (toNYlsia)
        {
            EnableCayoPerico(false);
            Wait(300);
            teleportLocation = NYlsiaTeleportLocation;
            LoadNY.UnloadNY(this);

        }
        else
        {
            // disable cayo, from cayo
            Wait(300);
            teleportLocation = lsiaTeleportLocation;
            EnableCayoPerico(false);

        }

        Wait(700);
        Function.Call(Hash.DO_SCREEN_FADE_IN, 800);
        // Teleport the player
        //Wait(200);
        Game.Player.Character.Position = teleportLocation;
        Wait(100);

        // If this was an automatic teleport, reset isFading after a short delay
        if (!manuallyTravelled)
        {
            Wait(500);
        }

        isFading = false;
    }
}
