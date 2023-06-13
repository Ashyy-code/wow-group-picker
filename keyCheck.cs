using System;
using System.Collections.Generic;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Net;

/// <summary>
/// Summary description for keyCheck
/// </summary>
[WebService(Namespace = "http://Ashypls.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class keyCheck : System.Web.Services.WebService
{

    //non public methods

    //dungeon list loader
    public DataTable dungeonList_np()
    {
        System.Threading.Thread.Sleep(1000);


        //open the stream reader to check the dungeon list - todo Update this
        StreamReader SR = new StreamReader(Server.MapPath("~/DBS/wow/wowDungeons.txt"));
        string str = SR.ReadToEnd();
        SR.Close();
        SR.Dispose();

        //setup the return table
        DataTable tbldungeonList = new DataTable();
        tbldungeonList.Columns.Add("dungeon_name");
        tbldungeonList.Columns.Add("era");
        tbldungeonList.Columns.Add("era_img");
        tbldungeonList.Columns.Add("dangerous_boss_tyran");
        tbldungeonList.Columns.Add("dangerous_trash_forti");
        tbldungeonList.Columns.Add("skippable_trash_shroud");
        tbldungeonList.Columns.Add("enraged_mobs");
        tbldungeonList.Columns.Add("important_purges");
        tbldungeonList.Columns.Add("low_graveyards");
        tbldungeonList.Columns.Add("era_sort");
        tbldungeonList.Columns.Add("big_snares");


        //split the list and create datarows
        string[] individualDungeons = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        //add dungeons to the list from the SR
        foreach (string dungeon in individualDungeons)
        {
            //get the individual dungeon items
            string[] dungeonDetails = dungeon.Split(new char[] { ';' }, StringSplitOptions.None);

            //add it to the list
            DataRow dungeonRow = tbldungeonList.NewRow();
            dungeonRow["dungeon_name"] = dungeonDetails[0];
            dungeonRow["era"] = dungeonDetails[1];
            dungeonRow["era_img"] = "https://ashypls.com/wowzers/img/eras/" + dungeonDetails[1] + ".png";
            dungeonRow["dangerous_boss_tyran"] = dungeonDetails[2];
            dungeonRow["dangerous_trash_forti"] = dungeonDetails[3];
            dungeonRow["skippable_trash_shroud"] = dungeonDetails[4];
            dungeonRow["enraged_mobs"] = dungeonDetails[5];
            dungeonRow["important_purges"] = dungeonDetails[6];
            dungeonRow["low_graveyards"] = dungeonDetails[7];
            dungeonRow["era_sort"] = dungeonDetails[8];
            dungeonRow["big_snares"] = dungeonDetails[9];
            tbldungeonList.Rows.Add(dungeonRow);
        }

        //return it
        DataView dungeonSort = tbldungeonList.DefaultView;
        dungeonSort.Sort = "era_sort ASC, dungeon_name ASC";
        return dungeonSort.ToTable();
    }

    //speclist loader
    public DataTable specList_np()
    {
        //open the stream reader to check the dungeon list - todo Update this
        StreamReader SR = new StreamReader(Server.MapPath("~/DBS/wow/specs_gp.json"));
        string str = SR.ReadToEnd();
        SR.Close();
        SR.Dispose();

        //return the JSON file as a datatable (for ease of parsing)
        return JsonConvert.DeserializeObject<DataTable>(str);
    }

    //reasonlist loader
    public DataTable reasonList_np()
    {
        //open the stream reader to check the dungeon list - todo Update this
        StreamReader SR = new StreamReader(Server.MapPath("~/DBS/wow/reasonings.json"));
        string str = SR.ReadToEnd();
        SR.Close();
        SR.Dispose();

        //return the JSON file as a datatable (for ease of parsing)
        return JsonConvert.DeserializeObject<DataTable>(str);
    }

    //classlist loader
    public DataTable classList_np()
    {
        //open the stream reader to check the dungeon list - todo Update this
        StreamReader SR = new StreamReader(Server.MapPath("~/DBS/wow/classes.json"));
        string str = SR.ReadToEnd();
        SR.Close();
        SR.Dispose();

        //return the JSON file as a datatable (for ease of parsing)
        return JsonConvert.DeserializeObject<DataTable>(str);
    }

    //player list loader
    public DataTable playerChars_np()
    {
        //create holder table for data
        DataTable playersTBL = new DataTable();
        playersTBL.Columns.Add(new DataColumn("player"));
        playersTBL.Columns.Add(new DataColumn("charName"));
        playersTBL.Columns.Add(new DataColumn("iLevel"));
        playersTBL.Columns.Add(new DataColumn("class"));
        playersTBL.Columns.Add(new DataColumn("class_img"));
        playersTBL.Columns.Add(new DataColumn("spec"));
        playersTBL.Columns.Add(new DataColumn("role"));
        playersTBL.Columns.Add(new DataColumn("spec_img"));
        playersTBL.Columns.Add(new DataColumn("spec_color"));
        playersTBL.Columns.Add(new DataColumn("score", typeof(Int32)));


        //set the webclient to grab the google doc
        WebClient webClient = new WebClient();
        string output = webClient.DownloadString("https://docs.google.com/spreadsheets/d/1qyRAGcQRb8ChtmQdNMgGmcKIGDpuIfVQbd9K-xShlVQ/edit?usp=sharing");

        //disgusting string splitting to format the output and scrape the data
        string[] splitter1 = { "<tbody>" };
        string[] mainOutupt = output.Split(splitter1, StringSplitOptions.None);
        string[] mainTwo = mainOutupt[1].Split(new string[] { "<tr" }, StringSplitOptions.None);

        //holder list for parsed lines
        List<string> lines = new List<string>();

        //check each line
        foreach (string line in mainTwo)
        {
            //only consider the row if it contains DPS,TANK,HEALER thus representing an actual row (Thanks dumbfucks for adding images :) )
            if ((line.Contains("DPS") || line.Contains("TANK") || line.Contains("HEALER")) && line.StartsWith(" style="))
            {
                //add this line to be parsed
                lines.Add(line);
            }
        }

        //itterate over each line and parse it
        foreach (string line in lines)
        {
            //split the line into items
            string[] lineItems = line.Split(new string[] { ">" }, StringSplitOptions.None);

            //create a new row in the holder Table and set the values
            DataRow playerRow = playersTBL.NewRow();
            playerRow["player"] = lineItems[6].Replace("</td", "");
            playerRow["charName"] = lineItems[8].Replace("</td", "");
            playerRow["iLevel"] = lineItems[10].Replace("</td", "");
            playerRow["class"] = lineItems[12].Replace("</td", "");
            playerRow["spec"] = lineItems[14].Replace("</td", "");
            playerRow["role"] = lineItems[16].Replace("</td", "");
            playerRow["score"] = 0;


            //find the spec img and color from the speclist
            DataTable specs = specList_np();
            foreach (DataRow row in specs.Rows)
            {
                if (row["spec"].ToString() == playerRow["spec"].ToString() + " " + playerRow["class"].ToString())
                {
                    playerRow["spec_img"] = row["img"].ToString();
                    playerRow["spec_color"] = row["color"].ToString();
                }
            }

            DataTable classes = classList_np();
            foreach (DataRow row in classes.Rows)
            {
                if (row["name"].ToString() == playerRow["class"].ToString())
                {
                    playerRow["class_img"] = row["img"].ToString();
                }
            }




            //add this row to the table
            playersTBL.Rows.Add(playerRow);

            //go to next
        }

        //return the fully parsed table of players
        return playersTBL;
    }

    //affix list loader
    public DataTable affixList_np()
    {
        //set the webclient connection to SSL (required by the site to be scraped (https://wowaffixes.info/))
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //setup the webclient to scrape this page
        WebClient webClient = new WebClient();
        string output = webClient.DownloadString("https://wowaffixes.info/");

        //disgusting string splitting to get to the data we want from the response.
        string[] items = output.Split(new string[] { "affixes-" }, StringSplitOptions.RemoveEmptyEntries);

        //holder list of un-parsed affixes
        List<string> list = new List<string>();

        //itterate over each item to be parsed
        foreach (string item in items)
        {
            //get the ID for the affix and add to list
            string affixNumber = item.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries)[0];
            list.Add(affixNumber);
        }

        //setup the list to hold this weeks affixes (not all affixes like above list)
        List<string> thisWeek = new List<string>();

        //add the affixes pulled from the above scrape to this list
        thisWeek.Add(list[1]);
        thisWeek.Add(list[2]);
        thisWeek.Add(list[3]);

        //create holder table for data - this will be the main JSON object returned
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("affix_id"));
        dt.Columns.Add(new DataColumn("affix_name"));
        dt.Columns.Add(new DataColumn("affix_icon"));
        dt.Columns.Add(new DataColumn("is_current"));


        //get all afixes
        //open and read the ALL Affixes textfile I scraped from wowhead
        StreamReader SR = new StreamReader(Server.MapPath("~/DBS/wow/affixes.txt"));
        string outputAf = SR.ReadToEnd();
        SR.Close();
        SR.Dispose();

        //parse the textfile into individual rows via splitting
        string[] individualAffxies = outputAf.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        //itterate over each item to parse it 
        foreach (string item in individualAffxies)
        {
            //get the ID
            string affix = item;

            //this is just vile dont read this line
            affix = affix.Replace(".affixes-", "").Replace(":before {    background-image: url", ";").Replace("(", "https://wowaffixes.info/").Replace(")", "");

            //more splitting to get the various parts
            string[] affixinfo = affix.Split(new char[] { ';' });
            string[] affixinfoDot = affix.Split(new char[] { '.' }, StringSplitOptions.None);
            string affixNameList = affixinfoDot[1];
            affixNameList = affixNameList.Replace("info//build/images/", "");

            //get holder vars
            int affixID = Int32.Parse(affixinfo[0]);
            string affixName = affixNameList;
            string affixIcon = "https://ashypls.com/wowzers/img/affixes/" + affixName + ".jpg";

            //create a new instance of an affix row
            DataRow dr = dt.NewRow();

            //set the items of this row from the parsed list of all affixes
            dr["affix_id"] = affixID;
            dr["affix_name"] = affixName;
            dr["affix_icon"] = affixIcon;

            //holder bool to see if this is one of the current affixes from the wowaffixes scraped url
            bool isCur = false;

            //itterate over the THE WEEK list made earlier, if the affix ID matches the one being looked at, then this affix is current
            foreach (string aID in thisWeek)
            {
                // set the bool flag if current
                if (Int32.Parse(aID) == affixID) { isCur = true; }
            }

            //set this is_current flag in the datatable
            dr["is_current"] = isCur;

            //add this row to the table
            dt.Rows.Add(dr);

            //goto next
        }

        //return this datatable.
        return dt;
    }

    //table to contain reasons for an individual player
    public DataTable player_reasoning_table()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("reason"));
        dt.Columns.Add(new DataColumn("reason_icon"));
        dt.Columns.Add(new DataColumn("reason_desc"));
        dt.Columns.Add(new DataColumn("reason_prio"));
        return dt;
    }


    //public methods

    //public method api for the dungeon list
    [WebMethod]
    public string dungeonList()
    {
        //Error catching
        try
        {
            //return the list of dungeons in JSON object form
            return HelperFunctions.requestResult(JsonConvert.SerializeObject(dungeonList_np()), "success");
        }
        //OOPSIE
        catch
        {
            //return the error status
            return HelperFunctions.requestResult("Could not load Dungeon List", "fail");
        }
    }



    //public method api for the player chars
    [WebMethod]
    public string playerChars()
    {
        //Error catching
        try
        {
            //return the list of players as JSON object
            return HelperFunctions.requestResult(JsonConvert.SerializeObject(playerChars_np()), "success");
        }
        //OOPSIE
        catch
        {
            //return the error status
            return HelperFunctions.requestResult("Could not load Player List", "fail");
        }
    }


    //public method api for the spec stats
    [WebMethod]
    public string specStats()
    {
        //Error catching
        try
        {
            //return the list of specs as JSON object
            return HelperFunctions.requestResult(JsonConvert.SerializeObject(specList_np()), "success");
        }
        //OOPSIE
        catch
        {
            //return the error status
            return HelperFunctions.requestResult("Could not load Player List", "fail");
        }
    }


    //public method API for the affix list
    [WebMethod]
    public string affixList()
    {
        //Error catching
        try
        {
            //return the list of affixes as JSON object
            return HelperFunctions.requestResult(JsonConvert.SerializeObject(affixList_np()), "success");
        }
        //OOPSIE
        catch
        {
            //return the error status
            return HelperFunctions.requestResult("Could not load Affix List", "fail");
        }
    }


    //Main logic algorythm for the computed group
    //public method api for the main check
    [WebMethod]
    public string computedGroup(
       string dungeonName,
       string keystoneLevel,
       string affix_1,
       string affix_2,
       string affix_3,
       string player_1_name,
       string player_2_name,
       string player_3_name,
       string player_4_name,
       string player_5_name)
    {

        //check for nulls
        bool validationPassed = true;

        //validation checking
        //none of these can be null
        if (dungeonName == null) { validationPassed = false; }
        if (keystoneLevel == null) { validationPassed = false; }
        if (affix_1 == null) { validationPassed = false; }
        if (affix_2 == null) { validationPassed = false; }
        if (affix_3 == null) { validationPassed = false; }

        //one of these cannot be null, the rest can
        if (player_1_name == "n/a") { validationPassed = false; }

        //check validation passed
        if (validationPassed == false)
        {
            //return the response that validation has failed
            return HelperFunctions.requestResult("You gotta supply all the fields lol. Only playerNames can be null, and keyowner is required", "fail");
        }

        //create the output table
        //this is the main table that will be returned as a JSON object when group is computed.
        DataTable options = new DataTable();
        options.Columns.Add(new DataColumn("player"));
        options.Columns.Add(new DataColumn("role"));
        options.Columns.Add(new DataColumn("role_card"));
        options.Columns.Add(new DataColumn("char"));
        options.Columns.Add(new DataColumn("icon"));
        options.Columns.Add(new DataColumn("color"));
        options.Columns.Add(new DataColumn("reasoning"));
        options.Columns.Add(new DataColumn("is_lust"));
        options.Columns.Add(new DataColumn("is_CR"));
        options.Columns.Add(new DataColumn("is_PI"));

        //create the reasonings table - class/spec specficic
        StreamReader SR = new StreamReader(Server.MapPath("~/DBS/wow/reasonings.json"));
        string class_rs = SR.ReadToEnd();
        SR.Close();
        DataTable class_reasonings = JsonConvert.DeserializeObject<DataTable>(class_rs);

        //do the actual computation next .. oh boy...

        //get the datasets
        DataTable tbl_affixes = affixList_np();
        DataTable dungeonList = dungeonList_np();
        DataTable reasonList = reasonList_np();
        DataTable playerChars = allPlayers().Copy();

        //check for what is needed
        //dungeon first

        //setup dungeon selected
        object selectedDungeon = null;

        //setup dungeon requirement flags
        int dangerous_boss_tyran = 0;
        int dangerous_trash_forti = 0;
        int skippable_trash_shroud = 0;
        int enraged_mobs = 0;
        int important_purges = 0;
        int low_graveyards = 0;
        int big_snares = 0;

        //get the selected dugeon from the table
        foreach (DataRow row in dungeonList.Rows)
        {
            if (row["dungeon_name"].ToString() == dungeonName)
            {
                //set the row
                selectedDungeon = row;

                //set the params
                dangerous_boss_tyran = Int32.Parse(row["dangerous_boss_tyran"].ToString());
                dangerous_trash_forti = Int32.Parse(row["dangerous_trash_forti"].ToString());
                skippable_trash_shroud = Int32.Parse(row["skippable_trash_shroud"].ToString());
                enraged_mobs = Int32.Parse(row["enraged_mobs"].ToString());
                important_purges = Int32.Parse(row["important_purges"].ToString());
                low_graveyards = Int32.Parse(row["low_graveyards"].ToString());
                big_snares = Int32.Parse(row["big_snares"].ToString());
            }
        }

        //check if nothing
        if (selectedDungeon == null)
        {
            //this dungeon name was not found
            return HelperFunctions.requestResult("Dungeon: " + dungeonName.ToString() + " was not found.", "fail");
        }

        //More setup for needed role variables
        int need_tank = 1;
        int need_healer = 1;
        int need_dps = 3;

        //hold the selected roles
        DataRow selectedTank = null;
        DataRow selectedHealer = null;
        DataRow selectedDPS1 = null;
        DataRow selectedDPS2 = null;
        DataRow selectedDPS3 = null;


        //MANUAL LOGIC BELOW
        //THUS BEGINS THE ALGORYTHM OF DEATH
        int want_high_aoe_dmg = 0;
        int want_high_st_dmg = 0;
        int want_lust = 1; //always want lust by default
        int want_cr = 1; //always want CR by default
        int want_pi = 0;
        int want_soothe = 0;
        int want_shroud = 0;
        int want_purge = 0;
        int want_summon = 0;
        int want_offheal = 0;
        int want_snare_remove = 0;
        int want_incorporeal = 0;
        int want_sanguine = 0;
        int want_afflicted = 0;
        int want_spiteful = 0;

        //ATTRIUBTE SCORE WEIGHTS HERE. ADJUST AS NEEDED
        //MODIFY THIS TO CHANGE HOW IMPORTANT EACH VALUE IS
        int high_aoe_dmg_score = 65;
        int high_st_dmg_score = 60;
        int lust_score = 120;
        int CR_score = 80;
        int pi_score = 20;
        int soothe_score = 60;
        int shroud_score = 18;
        int purge_score = 23;
        int summon_score = 8;
        int offheal_score = 16;
        int snare_remove_score = 31;
        int incorporeal_score = 32;
        int sanguine_score = 20;
        int afflicted_score = 30;
        int spiteful_score = 9;
        int good_kicks_score = 4;
        //END MODIFICATION ---------------------------------

        //flags to handle if we have lust./cr
        int have_lust = 0;
        int have_cr = 0;

        //combine affix names
        string affixGrouped = affix_1 + ";" + affix_2 + ";" + affix_3;

        //check weekly
        int is_tyran = 0;
        int is_forti = 0;

        //check em
        if (affixGrouped.ToLower().Contains("tyrannical")) { is_tyran = 1; } else { is_tyran = 0; };
        if (affixGrouped.ToLower().Contains("fortified")) { is_forti = 1; } else { is_forti = 0; };

        //flag checking
        //GENERATE THE REQUIRED FLAGS FOR THIS DUNEGON/AFFIX COMBO
        if (dangerous_boss_tyran == 1 && is_tyran == 1) { want_high_st_dmg = 1; want_pi = 1; want_offheal = 1; };
        if (dangerous_trash_forti == 1 && is_forti == 1) { want_high_aoe_dmg = 1; want_pi = 1; };
        if (enraged_mobs == 1) { want_soothe = 1; };
        if (skippable_trash_shroud == 1) { want_shroud = 1; };
        if (important_purges == 1) { want_purge = 1; };
        if (low_graveyards == 1) { want_summon = 1; };
        if (affixGrouped.ToLower().Contains("sanguine")) { want_sanguine = 1; };
        if (affixGrouped.ToLower().Contains("afflicted")) { want_afflicted = 1; };
        if (affixGrouped.ToLower().Contains("incorporeal")) { want_incorporeal = 1; };
        if (affixGrouped.ToLower().Contains("spiteful")) { want_spiteful = 1; };
        if (affixGrouped.ToLower().Contains("raging")) { want_soothe = 1; };
        if (affixGrouped.ToLower().Contains("entagling") || big_snares == 1) { want_snare_remove = 1; };


        //get all available chars based on the provided players
        foreach (DataRow charSpec in playerChars.Rows)
        {
            //get the flags
            int can_lust = Int32.Parse(charSpec["can_lust"].ToString());
            int can_purge = Int32.Parse(charSpec["can_purge"].ToString());
            int can_pi = Int32.Parse(charSpec["can_pi"].ToString());
            int can_shroud = Int32.Parse(charSpec["can_shroud"].ToString());
            int can_CR = Int32.Parse(charSpec["can_CR"].ToString());
            int can_offheal = Int32.Parse(charSpec["can_offheal"].ToString());
            int can_summon = Int32.Parse(charSpec["can_summon"].ToString());
            int good_incorporeal = Int32.Parse(charSpec["good_incorporeal"].ToString());
            int good_sanguine = Int32.Parse(charSpec["good_sanguine"].ToString());
            int good_raging = Int32.Parse(charSpec["good_raging"].ToString());
            int good_spiteful = Int32.Parse(charSpec["good_spiteful"].ToString());
            int good_afflicted = Int32.Parse(charSpec["good_afflicted"].ToString());
            int good_dmg_aoe = Int32.Parse(charSpec["good_dmg_aoe"].ToString());
            int good_dmg_st = Int32.Parse(charSpec["good_dmg_st"].ToString());
            int snare_removal = Int32.Parse(charSpec["snare_removal"].ToString());
            int good_kicks = Int32.Parse(charSpec["strong_interrupts_silence"].ToString());

            //manual flag checks
            //GENERATE THE CHARACTERS SCORE FOR THIS WEEK
            int scoreTally = 0;
            if (want_lust == 1 && have_lust == 0 && can_lust == 1) { scoreTally = scoreTally + lust_score; };
            if (want_cr == 1 && have_cr == 0 && can_CR == 1) { scoreTally = scoreTally + CR_score; };
            if (want_pi == 1 && can_pi == 1) { scoreTally = scoreTally + pi_score; };
            if (want_shroud == 1 && can_shroud == 1) { scoreTally = scoreTally + shroud_score; };
            if (want_purge == 1 && can_purge == 1) { scoreTally = scoreTally + purge_score; };
            if (want_offheal == 1 && can_offheal == 1) { scoreTally = scoreTally + offheal_score; };
            if (want_summon == 1 && can_summon == 1) { scoreTally = scoreTally + summon_score; };
            if (want_snare_remove == 1 && snare_removal == 1) { scoreTally = scoreTally + snare_remove_score; };
            if (want_incorporeal == 1 && good_incorporeal == 1) { scoreTally = scoreTally + incorporeal_score; };
            if (want_sanguine == 1 && good_sanguine == 1) { scoreTally = scoreTally + sanguine_score; };
            if (want_soothe == 1 && good_raging == 1) { scoreTally = scoreTally + soothe_score; };
            if (want_afflicted == 1 && good_afflicted == 1) { scoreTally = scoreTally + afflicted_score; };
            if (want_spiteful == 1 && good_spiteful == 1) { scoreTally = scoreTally + spiteful_score; };
            if (want_high_aoe_dmg == 1 && good_dmg_aoe == 1) { scoreTally = scoreTally + high_aoe_dmg_score; };
            if (want_high_st_dmg == 1 && good_dmg_st == 1) { scoreTally = scoreTally + high_st_dmg_score; };
            if (good_kicks == 1) { scoreTally = scoreTally + good_kicks_score; };

            charSpec["score"] = scoreTally;


        }

        //sort this table
        DataView dview = playerChars.DefaultView;
        dview.Sort = "score DESC";
        DataTable BestCharactersThisKey = playerChars.DefaultView.ToTable();

        //BestCharacterThisKey is now the primary chars table, ordered by which chars have scored highest for this dungeon/affix combo
        //now begin plucking characters from the top of this list depending on whats needed.

        //keystone owner
        // ✌ ✌ ✌ ✌ ✌ ✌ ✌ ✌ ✌ ✌ ✌ ✌ ✌ ✌ ✌ ✌
        //KEYSTONE OWNER TAKE FIRST PRIO - CHECK HERE
        DataView keystoneCharSpecsView = BestCharactersThisKey.Copy().DefaultView;
        keystoneCharSpecsView.RowFilter = "charName = '" + player_1_name + "'";
        DataTable keystoneCharSpecs = keystoneCharSpecsView.ToTable();
        //add this player to the options
        DataRow keystoneOwnerTopPick = keystoneCharSpecs.Rows[0];
        string keystoneOwnerRoleCard = "";
        //check role
        switch (keystoneOwnerTopPick["role"].ToString())
        {
            case "TANK":
                need_tank = need_tank - 1;
                keystoneOwnerRoleCard = "https://ashypls.com/wowzers/img/rolecards/tank_card.png";
                break;
            case "HEALER":
                need_healer = need_healer - 1;
                keystoneOwnerRoleCard = "https://ashypls.com/wowzers/img/rolecards/healer_card.png";
                break;
            case "DPS":
                keystoneOwnerRoleCard = "https://ashypls.com/wowzers/img/rolecards/dps" + need_dps + "_card.png";
                need_dps = need_dps - 1;
                break;
        }
        DataTable keystoneOwnerReasoning = player_reasoning_table().Copy();
        //keystone owner only
        keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 10);
        //other reasons
        if (want_lust == 1 && Int32.Parse(keystoneOwnerTopPick["can_lust"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 4); };
        if (want_cr == 1 && Int32.Parse(keystoneOwnerTopPick["can_CR"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 3); };
        if (want_pi == 1 && Int32.Parse(keystoneOwnerTopPick["can_pi"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 5); };
        if (want_shroud == 1 && Int32.Parse(keystoneOwnerTopPick["can_shroud"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 6); };
        if (want_purge == 1 && Int32.Parse(keystoneOwnerTopPick["can_purge"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 13); };
        if (want_offheal == 1 && Int32.Parse(keystoneOwnerTopPick["can_offheal"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 18); };
        if (want_summon == 1 && Int32.Parse(keystoneOwnerTopPick["can_summon"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 12); };
        if (want_snare_remove == 1 && Int32.Parse(keystoneOwnerTopPick["snare_removal"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 11); };
        if (want_incorporeal == 1 && Int32.Parse(keystoneOwnerTopPick["good_incorporeal"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 15); };
        if (want_sanguine == 1 && Int32.Parse(keystoneOwnerTopPick["good_sanguine"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 14); };
        if (want_soothe == 1 && Int32.Parse(keystoneOwnerTopPick["good_raging"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 9); };
        if (want_afflicted == 1 && Int32.Parse(keystoneOwnerTopPick["good_afflicted"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 16); };
        if (want_spiteful == 1 && Int32.Parse(keystoneOwnerTopPick["good_spiteful"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 17); };
        if (want_high_aoe_dmg == 1 && Int32.Parse(keystoneOwnerTopPick["good_dmg_aoe"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 1); };
        if (want_high_st_dmg == 1 && Int32.Parse(keystoneOwnerTopPick["good_dmg_st"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 2); };
        //generics
        if (keystoneOwnerTopPick["class"].ToString().ToLower() == "warlock") { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 7); };
        if (Int32.Parse(keystoneOwnerTopPick["strong_interrupts_silence"].ToString()) == 1) { keystoneOwnerReasoning = addReason(keystoneOwnerReasoning, reasonList, 8); };
        DataView keystoneOwnerReasoningView = keystoneOwnerReasoning.DefaultView;
        keystoneOwnerReasoningView.Sort = "reason_prio DESC";
        keystoneOwnerReasoning = keystoneOwnerReasoningView.ToTable();
        //get the reasons into json
        String keystoneOwnerReasons = JsonConvert.SerializeObject(keystoneOwnerReasoning);

        //add this char to the options table 
        DataRow option_1 = options.NewRow();
        option_1["player"] = keystoneOwnerTopPick["player"].ToString();
        option_1["role"] = keystoneOwnerTopPick["role"].ToString();
        option_1["role_card"] = keystoneOwnerRoleCard;
        option_1["char"] = keystoneOwnerTopPick["charName"].ToString();
        option_1["icon"] = keystoneOwnerTopPick["spec_img"].ToString();
        option_1["color"] = keystoneOwnerTopPick["spec_color"].ToString();
        option_1["reasoning"] = keystoneOwnerReasons;
        option_1["is_lust"] = keystoneOwnerTopPick["can_lust"].ToString();
        option_1["is_CR"] = keystoneOwnerTopPick["can_CR"].ToString();
        option_1["is_PI"] = keystoneOwnerTopPick["can_pi"].ToString();
        //add row
        options.Rows.Add(option_1);

        //remove this player from the keylist
        DataView bkvNew = BestCharactersThisKey.DefaultView;
        bkvNew.RowFilter = "NOT player = '" + keystoneOwnerTopPick["player"].ToString() + "'";
        BestCharactersThisKey = bkvNew.ToTable();


        //ADD THE NON-SELECTED AND ONLY-SELECTED PLAYERS NOW... WAHOO CANT FUCKING WAIT FOR THIS
        //LETSAGO -_-
        /////////////////////////////////
        //──────────────███████──███████
        //──────────████▓▓▓▓▓▓████░░░░░██
        //────────██▓▓▓▓▓▓▓▓▓▓▓▓██░░░░░░██
        //──────██▓▓▓▓▓▓████████████░░░░██
        //────██▓▓▓▓▓▓████████████████░██
        //────██▓▓████░░░░░░░░░░░░██████
        //──████████░░░░░░██░░██░░██▓▓▓▓██
        //──██░░████░░░░░░██░░██░░██▓▓▓▓██
        //██░░░░██████░░░░░░░░░░░░░░██▓▓██
        //██░░░░░░██░░░░██░░░░░░░░░░██▓▓██
        //──██░░░░░░░░░███████░░░░██████
        //────████░░░░░░░███████████▓▓██
        //──────██████░░░░░░░░░░██▓▓▓▓██
        //────██▓▓▓▓██████████████▓▓██
        //──██▓▓▓▓▓▓▓▓████░░░░░░████
        //████▓▓▓▓▓▓▓▓██░░░░░░░░░░██
        //████▓▓▓▓▓▓▓▓██░░░░░░░░░░██
        //██████▓▓▓▓▓▓▓▓██░░░░░░████████
        //──██████▓▓▓▓▓▓████████████████
        //────██████████████████████▓▓▓▓██
        //──██▓▓▓▓████████████████▓▓▓▓▓▓██
        //████▓▓██████████████████▓▓▓▓▓▓██
        //██▓▓▓▓██████████████████▓▓▓▓▓▓██
        //██▓▓▓▓██████████──────██▓▓▓▓████
        //██▓▓▓▓████──────────────██████ 
        //──████
        //load any selected players
        DataView suppliedCharacterSpecsView = BestCharactersThisKey.Copy().DefaultView;
        suppliedCharacterSpecsView.RowFilter = "player in ('" + player_2_name + "','" + player_3_name + "','" + player_4_name + "','" + player_5_name + "')";
        DataTable suppliedCharacterList = suppliedCharacterSpecsView.ToTable();

        //load all non selected players
        DataView unSuppliedCharacterSpecsView = BestCharactersThisKey.Copy().DefaultView;
        unSuppliedCharacterSpecsView.RowFilter = "NOT player in ('" + player_2_name + "','" + player_3_name + "','" + player_4_name + "','" + player_5_name + "')";
        DataTable unSuppliedCharacterList = unSuppliedCharacterSpecsView.ToTable();

        //4 roles remaining at this point
        for (int i = 1; i <= 4; i++)
        {
            //prep the selected player
            DataRow selectedPlayer = null;

            //the same setup as the keystone owner
            string playerOwnerRoleCard = "";

            //whilever there are rows in the suppliedCharacterList, we must pluck players from there.
            if (suppliedCharacterList.Rows.Count > 0)
            {
                //players still need assigning
                //check tank first
                if ((need_tank > 0) && (selectedPlayer == null))
                {
                    DataView tanksOnly = suppliedCharacterList.DefaultView;
                    tanksOnly.RowFilter = "role = 'TANK'";
                    //check if we have any tanks in here
                    if (tanksOnly.ToTable().Rows.Count > 0)
                    {
                        //we have our tank
                        selectedPlayer = tanksOnly.ToTable().Rows[0];
                        need_tank = need_tank - 1;
                        playerOwnerRoleCard = "https://ashypls.com/wowzers/img/rolecards/tank_card.png";
                        //remove this player from the row.
                        DataView withoutPlayer = suppliedCharacterList.DefaultView;
                        withoutPlayer.RowFilter = "NOT player = '" + selectedPlayer["player"].ToString() + "'";
                        suppliedCharacterList = withoutPlayer.ToTable();
                    }
                }

                //check healer second
                if ((need_healer > 0) && (selectedPlayer == null))
                {
                    DataView healersOnly = suppliedCharacterList.DefaultView;
                    healersOnly.RowFilter = "role = 'HEALER'";
                    //check if we have any tanks in here
                    if (healersOnly.ToTable().Rows.Count > 0)
                    {
                        //we have our healer
                        selectedPlayer = healersOnly.ToTable().Rows[0];
                        need_healer = need_healer - 1;
                        playerOwnerRoleCard = "https://ashypls.com/wowzers/img/rolecards/healer_card.png";
                        //remove this player from the row.
                        DataView withoutPlayer = suppliedCharacterList.DefaultView;
                        withoutPlayer.RowFilter = "NOT player = '" + selectedPlayer["player"].ToString() + "'";
                        suppliedCharacterList = withoutPlayer.ToTable();
                    }
                }

                //check dps third
                if ((need_dps > 0) && (selectedPlayer == null))
                {
                    DataView dpsOnly = suppliedCharacterList.DefaultView;
                    dpsOnly.RowFilter = "role = 'DPS'";
                    //check if we have any tanks in here
                    if (dpsOnly.ToTable().Rows.Count > 0)
                    {
                        //we have our healer
                        selectedPlayer = dpsOnly.ToTable().Rows[0];
                        need_dps = need_dps - 1;
                        playerOwnerRoleCard = "https://ashypls.com/wowzers/img/rolecards/dps" + need_dps + "_card.png";
                        //remove this player from the row.
                        DataView withoutPlayer = suppliedCharacterList.DefaultView;
                        withoutPlayer.RowFilter = "NOT player = '" + selectedPlayer["player"].ToString() + "'";
                        suppliedCharacterList = withoutPlayer.ToTable();
                    }
                }

                //END SUPPLIED PLAYER PLUCKING

            }
            else
            {
                //players are now all assigned, we pluck from the non-assigned players table
                //players still need assigning
                //check tank first
                if ((need_tank > 0) && (selectedPlayer == null))
                {
                    DataView tanksOnly = unSuppliedCharacterList.DefaultView;
                    tanksOnly.RowFilter = "role = 'TANK'";
                    //check if we have any tanks in here
                    if (tanksOnly.ToTable().Rows.Count > 0)
                    {
                        //we have our tank
                        selectedPlayer = tanksOnly.ToTable().Rows[0];
                        need_tank = need_tank - 1;
                        playerOwnerRoleCard = "https://ashypls.com/wowzers/img/rolecards/tank_card.png";
                        //remove this player from the row.
                        DataView withoutPlayer = unSuppliedCharacterList.DefaultView;
                        withoutPlayer.RowFilter = "NOT player = '" + selectedPlayer["player"].ToString() + "'";
                        unSuppliedCharacterList = withoutPlayer.ToTable();
                    }
                }

                //check healer second
                if ((need_healer > 0) && (selectedPlayer == null))
                {
                    DataView healersOnly = unSuppliedCharacterList.DefaultView;
                    healersOnly.RowFilter = "role = 'HEALER'";
                    //check if we have any tanks in here
                    if (healersOnly.ToTable().Rows.Count > 0)
                    {
                        //we have our healer
                        selectedPlayer = healersOnly.ToTable().Rows[0];
                        need_healer = need_healer - 1;
                        playerOwnerRoleCard = "https://ashypls.com/wowzers/img/rolecards/healer_card.png";
                        //remove this player from the row.
                        DataView withoutPlayer = unSuppliedCharacterList.DefaultView;
                        withoutPlayer.RowFilter = "NOT player = '" + selectedPlayer["player"].ToString() + "'";
                        unSuppliedCharacterList = withoutPlayer.ToTable();
                    }
                }

                //check dps third
                if ((need_dps > 0) && (selectedPlayer == null))
                {
                    DataView dpsOnly = unSuppliedCharacterList.DefaultView;
                    dpsOnly.RowFilter = "role = 'DPS'";
                    //check if we have any tanks in here
                    if (dpsOnly.ToTable().Rows.Count > 0)
                    {
                        //we have our healer
                        selectedPlayer = dpsOnly.ToTable().Rows[0];
                        need_dps = need_dps - 1;
                        playerOwnerRoleCard = "https://ashypls.com/wowzers/img/rolecards/dps" + need_dps + "_card.png";
                        //remove this player from the row.
                        DataView withoutPlayer = unSuppliedCharacterList.DefaultView;
                        withoutPlayer.RowFilter = "NOT player = '" + selectedPlayer["player"].ToString() + "'";
                        unSuppliedCharacterList = withoutPlayer.ToTable();
                    }
                }
            }

            //TIME TO ADD THIS PLAYER
            if (selectedPlayer == null)
            {
                //Something has gone terribly wrong here
                return HelperFunctions.requestResult("Oh lord the algo gone goofed, pray for me.", "fail");
            }

            //Same setup again
            //Add this player
            DataTable playerOwnerReasoning = player_reasoning_table().Copy();
            //select reasons reasons
            if (want_lust == 1 && Int32.Parse(selectedPlayer["can_lust"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 4); };
            if (want_cr == 1 && Int32.Parse(selectedPlayer["can_CR"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 3); };
            if (want_pi == 1 && Int32.Parse(selectedPlayer["can_pi"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 5); };
            if (want_shroud == 1 && Int32.Parse(selectedPlayer["can_shroud"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 6); };
            if (want_purge == 1 && Int32.Parse(selectedPlayer["can_purge"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 13); };
            if (want_offheal == 1 && Int32.Parse(selectedPlayer["can_offheal"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 18); };
            if (want_summon == 1 && Int32.Parse(selectedPlayer["can_summon"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 12); };
            if (want_snare_remove == 1 && Int32.Parse(selectedPlayer["snare_removal"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 11); };
            if (want_incorporeal == 1 && Int32.Parse(selectedPlayer["good_incorporeal"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 15); };
            if (want_sanguine == 1 && Int32.Parse(selectedPlayer["good_sanguine"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 14); };
            if (want_soothe == 1 && Int32.Parse(selectedPlayer["good_raging"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 9); };
            if (want_afflicted == 1 && Int32.Parse(selectedPlayer["good_afflicted"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 16); };
            if (want_spiteful == 1 && Int32.Parse(selectedPlayer["good_spiteful"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 17); };
            if (want_high_aoe_dmg == 1 && Int32.Parse(selectedPlayer["good_dmg_aoe"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 1); };
            if (want_high_st_dmg == 1 && Int32.Parse(selectedPlayer["good_dmg_st"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 2); };
            //generics
            if (selectedPlayer["class"].ToString().ToLower() == "warlock") { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 7); };
            if (Int32.Parse(selectedPlayer["strong_interrupts_silence"].ToString()) == 1) { playerOwnerReasoning = addReason(playerOwnerReasoning, reasonList, 8); };
            DataView playerReasoningView = playerOwnerReasoning.DefaultView;
            playerReasoningView.Sort = "reason_prio DESC";
            playerOwnerReasoning = playerReasoningView.ToTable();
            //get the reasons into json
            String playerOwnerReasons = JsonConvert.SerializeObject(playerOwnerReasoning);

            //add this char to the options table 
            DataRow option = options.NewRow();
            option["player"] = selectedPlayer["player"].ToString();
            option["role"] = selectedPlayer["role"].ToString();
            option["role_card"] = playerOwnerRoleCard;
            option["char"] = selectedPlayer["charName"].ToString();
            option["icon"] = selectedPlayer["spec_img"].ToString();
            option["color"] = selectedPlayer["spec_color"].ToString();
            option["reasoning"] = playerOwnerReasons;
            option["is_lust"] = selectedPlayer["can_lust"].ToString();
            option["is_CR"] = selectedPlayer["can_CR"].ToString();
            option["is_PI"] = selectedPlayer["can_pi"].ToString();
            //add row
            options.Rows.Add(option);


        }

        //return the options
        return HelperFunctions.requestResult(JsonConvert.SerializeObject(options), "success");

    }

    //add a reason to the table of reasons for a given player
    public DataTable addReason(DataTable tbl_to_add_reason_to, DataTable reasonsTbl, int reason_id)
    {
        //get the reason based on id
        DataView dvw = reasonsTbl.DefaultView;
        dvw.RowFilter = "reasoning_id = " + reason_id;
        DataRow reason = dvw.ToTable().Rows[0];

        DataRow newReason = tbl_to_add_reason_to.NewRow();
        newReason["reason"] = reason["reason"].ToString();
        newReason["reason_icon"] = reason["reason_icon"].ToString();
        newReason["reason_desc"] = reason["reason_desc"].ToString();
        newReason["reason_prio"] = reason["reason_priority"].ToString();

        //add the row to the player reason table
        tbl_to_add_reason_to.Rows.Add(newReason);
        //return the table back
        return tbl_to_add_reason_to;
    }

    //The master table for all players with their specs and etc.
    public DataTable allPlayers()
    {
        DataTable allPlayers = playerChars_np().Copy();
        DataTable specs = specList_np();

        //add the spec specfics to this table of player chars
        allPlayers.Columns.Add(new DataColumn("can_lust"));
        allPlayers.Columns.Add(new DataColumn("can_purge"));
        allPlayers.Columns.Add(new DataColumn("can_pi"));
        allPlayers.Columns.Add(new DataColumn("can_shroud"));
        allPlayers.Columns.Add(new DataColumn("can_CR"));
        allPlayers.Columns.Add(new DataColumn("can_offheal"));
        allPlayers.Columns.Add(new DataColumn("good_incorporeal"));
        allPlayers.Columns.Add(new DataColumn("good_sanguine"));
        allPlayers.Columns.Add(new DataColumn("good_raging"));
        allPlayers.Columns.Add(new DataColumn("good_spiteful"));
        allPlayers.Columns.Add(new DataColumn("good_afflicted"));
        allPlayers.Columns.Add(new DataColumn("good_dmg_aoe"));
        allPlayers.Columns.Add(new DataColumn("good_dmg_st"));
        allPlayers.Columns.Add(new DataColumn("can_summon"));
        allPlayers.Columns.Add(new DataColumn("strong_interrupts_silence"));
        allPlayers.Columns.Add(new DataColumn("snare_removal"));

        foreach (DataRow charRow in allPlayers.Rows)
        {
            //find the spec
            DataRow thisCharSpec = null;

            //itterate
            foreach (DataRow specRow in specs.Rows)
            {
                if (specRow["class"].ToString() == charRow["class"].ToString() && specRow["role"].ToString() == charRow["role"].ToString())
                {
                    thisCharSpec = specRow;
                }
            }

            //set the new columns
            charRow["can_lust"] = thisCharSpec["can_lust"];
            charRow["can_purge"] = thisCharSpec["can_purge"];
            charRow["can_pi"] = thisCharSpec["can_pi"];
            charRow["can_shroud"] = thisCharSpec["can_shroud"];
            charRow["can_CR"] = thisCharSpec["can_CR"];
            charRow["can_offheal"] = thisCharSpec["can_offheal"];
            charRow["good_incorporeal"] = thisCharSpec["good_incorporeal"];
            charRow["good_sanguine"] = thisCharSpec["good_sanguine"];
            charRow["good_raging"] = thisCharSpec["good_raging"];
            charRow["good_spiteful"] = thisCharSpec["good_spiteful"];
            charRow["good_afflicted"] = thisCharSpec["good_afflicted"];
            charRow["good_dmg_aoe"] = thisCharSpec["good_dmg_aoe"];
            charRow["good_dmg_st"] = thisCharSpec["good_dmg_st"];
            charRow["can_summon"] = thisCharSpec["can_summon"];
            charRow["strong_interrupts_silence"] = thisCharSpec["strong_interrupts_silence"];
            charRow["snare_removal"] = thisCharSpec["snare_removal"];

        }
        return allPlayers;
    }


}
