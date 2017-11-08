using EasyHttp.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Windows.Forms;
using System.Threading;

namespace RuneReformed1._0._15
{
    public partial class Form1 : Form
    {
        public static string token;
        public static string port;
        public static HttpClient http;
        private String apiBaseURL = "http://api.riport.net:8080";
        private String publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA4qobG2BxMZ9EvNmw9eY02FYNuIth6ttl4dVHpKYA+hiFX/A//Aqy30F0x8MNCQLBMMIsc205/0vmD7YuFPdfoLLHFMolob37y3Fx5emAN/Xqjqoo5vfw37UzdEpd8Ok2LT8N75ot2BJkodQ0SIoLW4WxJPwPkS40/a42ehD4TZFAzvCgEHf1IMJTzI+hj1MAsKiUHE/qd/ggKJmE2U4xGjgVIGhh81hjBaV56/InsBJhGxygra0BtTp5a2dXOfj/EsCqeg6++EbWD9awBsapZ9sluPaGAbAO2vskXZJ4KGbBriymWMZo9lvagTxy/oTF+sEFuJtfUMvLOnBMJMcCKwIDAQAB";
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        private String platformId;
        private String summonerName;
        private int accountId;
        private int summonerId;

        public class ChampionData
        {
            public String uuid { get; set; }
            public String displayName { get; set; }

            public ChampionData(String uuid, String displayName)
            {
                this.uuid = uuid;
                this.displayName = displayName;
            }
        }

        public class RuneData
        {
            public String uuid { get; set; }
            public String championUuid { get; set; }
            public String displayName { get; set; }

            public RuneData(String uuid, String championUuid, String displayName)
            {
                this.uuid = uuid;
                this.championUuid = championUuid;
                this.displayName = displayName;
            }
        }

        public static List<ChampionData> Champions = new List<ChampionData>();
        public static List<RuneData> Runes = new List<RuneData>();

        public Form1()
        {
            http = new HttpClient();
            http.Request.Accept = HttpContentTypes.ApplicationJson;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var process = Process.GetProcessesByName("LeagueClientUx");
            if (process.Length == 0)
            {
                string msgbox =
                    "Could not find the League of Legends process, is League of Legends running?";
                MessageBox.Show(msgbox);
                this.Close();
            }
            else
            {
                foreach (var getid in process)
                {
                    using (ManagementObjectSearcher mos = new ManagementObjectSearcher(
                        "SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + getid.Id))
                    {
                        foreach (ManagementObject mo in mos.Get())
                        {
                            string data = (mo["CommandLine"].ToString());
                            string[] CommandlineArray = data.Split('"');

                            foreach (var atributes in CommandlineArray)
                            {
                                if (atributes.Contains("token"))
                                {
                                    string[] token = atributes.Split('=');
                                    Form1.token = token[1];
                                }
                                if (atributes.Contains("port"))
                                {
                                    string[] port = atributes.Split('=');
                                    Form1.port = port[1];
                                }
                            }
                        }
                    }
                }

                http.Request.SetBasicAuthentication("riot", token);
                var getregion =
                    http.Get("https://localhost:" + port + "/riotclient/v1/bugsplat/platform-id");
                //label1.Text  = getregion.DynamicBody; <-- region

                var getsummonername =
                    http.Get("https://localhost:" + port + "/lol-summoner/v1/current-summoner");
                var summonerinfo = getsummonername.DynamicBody;

                this.platformId = getregion.DynamicBody;

                foreach (KeyValuePair<string, object> x in summonerinfo)
                {
                    if (x.Key == "displayName")
                        summonerName = x.Value.ToString();
                    if (x.Key == "accountId")
                        accountId = (int)x.Value;
                    if (x.Key == "summonerId")
                        summonerId = (int)x.Value;

                }
                ThreadStart ts = new ThreadStart(loadRunesAndChampions);
                Thread loader = new Thread(ts);
                loader.Start();
                ThreadStart tts = new ThreadStart(sendTelemetryCall);
                Thread telemetry = new Thread(tts);
                telemetry.Start();
            }
        }

        public void loadRunesAndChampions()
        {
            try
            {



            var responsechampionlist = http.Get(this.apiBaseURL + "/rr-api/v2/champions");
            var championlist = responsechampionlist.DynamicBody;

            foreach (var y in championlist)
            {
                addItemToChampBox(this, ChampBox, y.displayName);
                Champions.Add(new ChampionData(y.uuid, y.displayName));
            }

            var responseapi = http.Get(this.apiBaseURL + "/rr-api/v2/runes");
            var getchamps = responseapi.DynamicBody;

                foreach (var x in getchamps)
                {
                    RuneData rd = new RuneData(x.uuid, x.championUuid, x.displayName);
                    Runes.Add(rd);
                }

                setBoxEnabled(this, ChampBox, true);
                setBoxEnabled(this, RuneBox, true);
            }
            catch (Exception e)
            {
                MessageBox.Show("Looks like the API is unavailable, try again later.");
                this.Close();
            }
        }

        public void sendTelemetryCall()
        {
            String json = "{\"application\":\"RunesReformed\",\"summonerName\":\"" + summonerName + "\",\"accountId\":" + accountId + ",\"summonerId\":" + summonerId + ",\"platformId\":\"" + platformId + "\"}";
            var telHttp = new HttpClient();
            telHttp.Request.Accept = HttpContentTypes.ApplicationJson;

            //String enc = ReportMyTeam.RMTEncrypt.EncryptText(json, publicKey);
            telHttp.Post(apiBaseURL + "/application-telemetry/v1/submit", json, HttpContentTypes.ApplicationJson);
        }

        delegate void SetBoxEnabledCallback(Form form, Control ctrl, Boolean state);
        public void setBoxEnabled(Form form, Control ctrl, Boolean state)
        {
            if (ctrl.InvokeRequired)
            {
                SetBoxEnabledCallback sbec = new SetBoxEnabledCallback(setBoxEnabled);
                ctrl.Invoke(sbec, new object[] { form, ctrl, state });
            }
            else
            {
                ctrl.Enabled = state;
            }
        }

        delegate void AddItemToChampBoxCallback(Form form, ComboBox ctrl, String item);
        public void addItemToChampBox(Form form, ComboBox ctrl, String item)
        {
            if (ctrl.InvokeRequired)
            {
                AddItemToChampBoxCallback adtcbc = new AddItemToChampBoxCallback(addItemToChampBox);
                ctrl.Invoke(adtcbc, new object[] { form, ctrl, item });
            }
            else
            {
                ctrl.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Interval = 3000;
            timer.Tick += timer_Tick;
            timer.Start();
            SetRunes.Enabled = false;
            try
            {
                String selectedPage = RuneBox.SelectedItem.ToString();
                String runeUuid = "";
                foreach (RuneData rd in Runes)
                    if (rd.displayName == selectedPage)
                        runeUuid = rd.uuid;

                string champion = ChampBox.SelectedItem.ToString();
                string champUUID = "";

                foreach (ChampionData cd in Champions)
                    if (champion == cd.displayName)
                        champUUID = cd.uuid;

                var responseRunes = Form1.http.Get(this.apiBaseURL + "/rr-api/v2/champions/" + champUUID + "/runes/" + runeUuid);
                var runeResponseBody = responseRunes.DynamicBody;

                int Runestart = runeResponseBody.styleOne;
                string name = runeResponseBody.pageName;
                int rune1 = runeResponseBody.perkOne;
                int rune2 = runeResponseBody.perkTwo;
                int rune3 = runeResponseBody.perkThree;
                int rune4 = runeResponseBody.perkFour;
                int rune5 = runeResponseBody.perkFive;
                int rune6 = runeResponseBody.perkSix;
                int secondary = runeResponseBody.styleTwo;

                var inputLCUx = @"{""name"":""" + "RR - " + name + "\",\"primaryStyleId\":" + Runestart + ",\"selectedPerkIds\": [" +
                                rune1 + "," + rune2 + "," + rune3 + "," + rune4 + "," + rune5 + "," + rune6 +
                                "],\"subStyleId\":" + secondary + "}";

                string password = token;

                var riotHttp = new HttpClient();
                riotHttp.Request.Accept = HttpContentTypes.ApplicationJson;
                riotHttp.Request.SetBasicAuthentication("riot", password);

                riotHttp.Post("https://127.0.0.1:" + port + "/lol-perks/v1/pages", inputLCUx, HttpContentTypes.ApplicationJson);
            }
            catch (Exception f)
            {
                MessageBox.Show("Something somewhere went wrong, show this to the admin " + f.Message);
            }



        }

        private void ChampBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RuneBox.Items.Clear();
            string champion = ChampBox.SelectedItem.ToString();
            string champUUID = "";

            foreach (ChampionData cd in Champions)
                if (champion == cd.displayName)
                    champUUID = cd.uuid;

            foreach (RuneData rd in Runes)
                if (rd.championUuid == champUUID)
                    RuneBox.Items.Add(rd.displayName);
        }

        void timer_Tick(object sender, System.EventArgs e)
        {
            SetRunes.Enabled = true;
            timer.Stop();
        }
    }
}