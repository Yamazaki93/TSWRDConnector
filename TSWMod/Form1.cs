using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using TSWMod.RailDriver;
using TSWMod.TSW;
using TSWMod.TSW.NEC;

namespace TSWMod
{
    public partial class Form1 : Form
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private const string SettingsFile = "settings.json";


        public Form1()
        {
            InitializeApplicationSettings();

            _rdConnector = new RailDriverConnector();
            _tswConnector = new TSWConnector(_rdConnector);

            InitializeComponent();

            // UI Initialization
            lblMap.Text = "-";
            lblRDStatus.Text = "";
            lblTSWStatus.Text = "";
            panelRdControls.Visible = false;
            lblLoadCalibrationPrompt.Visible = true;
            lblActiveTrain.Text = "-";
            groupBoxTrainConfig.Visible = false;

            _rdConnector.Connected += RdConnectorOnConnected;
            _tswConnector.StatusChanged += TSWConnectorOnConnected; 
            _tswConnector.MapChanged += TSWConnectorMapChanged; 
            _rdConnector.LeverChanged += RdConnectorOnLeverChanged;
            _tswConnector.LocomotiveChanged += LocomotiveChanged;

            TryLoadRDCalibration();
        }

        private void TryLoadRDCalibration()
        {
            if (!string.IsNullOrWhiteSpace(_settings.RDCalibrationPath))
            {
                var success = _rdConnector.LoadCalibrationFile(_settings.RDCalibrationPath);
                if (success)
                {
                    lblLoadCalibrationPrompt.Visible = false;
                    panelRdControls.Visible = true;
                }
            }
        }

        private void InitializeApplicationSettings()
        {
            if (File.Exists(SettingsFile))
            {
                try
                {
                    var jsonText = File.ReadAllText(SettingsFile);
                    _settings = JsonConvert.DeserializeObject<ApplicationSettings>(jsonText);
                }
                catch (Exception e)
                {
                    Logger.Warn(e, "Re-initialize settings.");
                    _settings = new ApplicationSettings();
                    SaveSettings();
                }
            }
            else
            {
                _settings = new ApplicationSettings();
                SaveSettings();
            }
        }

        private void LocomotiveChanged(object sender, TSWActiveLocomotiveChangedEvent e)
        {
            lblActiveTrain.BeginInvoke(new Action(() => { lblActiveTrain.Text = e.LocomotiveName; }));
            _currentTrain = e.LocomotiveName;
            groupBoxTrainConfig.BeginInvoke(new Action(() =>
            {
                if (_tswConnector.GetCurrentLocomotiveConfig() != null)
                {
                    groupBoxTrainConfig.Visible = true;
                    groupBoxTrainConfig.Text = _currentTrain;
                    ShowTrainConfig();
                }
                else
                {
                    groupBoxTrainConfig.Visible = false;
                }
            }));
        }

        private void ShowTrainConfig()
        {
            // Hide all train configs
            panelACS64Config.Visible = false;

            try
            {
                if (_currentTrain == "ACS-64")
                {
                    var cfg = _tswConnector.GetCurrentLocomotiveConfig();
                    panelACS64Config.Visible = true;
                    checkboxACSConfigMasterController.Checked = cfg[ACS_64.MasterControllerConfigKey].Value<string>()
                        .Equals(ACS_64.MasterControllerFollowTSW);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private void checkboxACSConfigMasterController_CheckedChanged(object sender, EventArgs e)
        {
            if (_currentTrain == "ACS-64")
            {
                SetACS64Config();
            }
        }

        private void SetACS64Config()
        {
            var cfg = new JObject
            {
                {
                    ACS_64.MasterControllerConfigKey,
                    checkboxACSConfigMasterController.Checked
                        ? ACS_64.MasterControllerFollowTSW
                        : ACS_64.MasterControllerFollowRD
                }
            };
            _tswConnector.SetCurrentLocomotiveConfig(cfg);
        }

        private void RdConnectorOnLeverChanged(object sender, RailDriverLeverUpdatedEvent e)
        {
            panelRdControls.BeginInvoke(new Action(() =>
            {
                lblReverserValue.Text = e.RailDriverLeverState.Reverser.ToString("F5");
                lblThrottleValue.Text = e.RailDriverLeverState.Throttle.ToString("F5");
                lblAutoBrakeValue.Text = e.RailDriverLeverState.AutoBrake.ToString("F5");
                lblIndependentBrakeValue.Text = e.RailDriverLeverState.IndependentBrake.ToString("F5");
            }));
        }

        private void TSWConnectorMapChanged(object sender, TSWMapChangedEvent e)
        {
            lblMap.BeginInvoke(new Action(() => { lblMap.Text = e.MapName; }));
        }

        private void TSWConnectorOnConnected(object sender, TSWConnectionChangedEvent e)
        {
            lblTSWStatus.BeginInvoke(new Action(() =>
            {
                switch (e.Status)
                {
                    case TSWConnectorStatus.NotConnected:
                        lblTSWStatus.Text = "Not Connected";
                        _rdConnector.SetPersistentMessage("rd");
                        lblTSWStatus.ForeColor = Color.Red;
                        break;
                    case TSWConnectorStatus.Calibrating:
                        lblTSWStatus.Text = "Calibrating";
                        lblTSWStatus.ForeColor = Color.Blue;
                        _rdConnector.SetPersistentMessage("CL");
                        break;
                    case TSWConnectorStatus.Ready:
                        lblTSWStatus.Text = "Train Connected";
                        lblTSWStatus.ForeColor = Color.Green;
                        _rdConnector.SetPersistentMessage("rd");
                        break;
                    case TSWConnectorStatus.WaitingForGame:
                        lblTSWStatus.Text = "Waiting For Session";
                        lblTSWStatus.ForeColor = Color.OrangeRed;
                        _rdConnector.FlashStatus("- - - - - -");
                        _rdConnector.SetPersistentMessage("rd");
                        break;
                    default:
                        lblTSWStatus.Text = "-";
                        break;
                }
            }));
        }

        private void RdConnectorOnConnected(object sender, RailDriverConnectionChangedEvent e)
        {
            lblRDStatus.BeginInvoke(new Action(() =>
            {
                if (e.Connected)
                {
                    lblRDStatus.Text = "Connected";
                    lblRDStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblRDStatus.Text = "Not Connected";
                    lblRDStatus.ForeColor = Color.Red;
                }
            }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rdConnector.CloseConnection();
            _tswConnector.CloseConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _rdConnector.OpenConnection();
            _tswConnector.OpenConnection();
        }

        private void btnLoadRDCalibration_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "fdm";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Multiselect = false;
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                var success = _rdConnector.LoadCalibrationFile(openFileDialog1.FileName);
                if (success)
                {
                    lblLoadCalibrationPrompt.Visible = false;
                    panelRdControls.Visible = true;
                    _settings.RDCalibrationPath = openFileDialog1.FileName;
                    SaveSettings();
                }
            }
        }

        private void SaveSettings()
        {
            File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(_settings));
        }

        private readonly RailDriverConnector _rdConnector;
        private readonly TSWConnector _tswConnector;
        private ApplicationSettings _settings;
        private string _currentTrain;

    }
}
