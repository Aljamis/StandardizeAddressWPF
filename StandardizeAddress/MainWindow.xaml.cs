using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using StandardizeAddress.AddrService;


namespace StandardizeAddress
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        AddrService.StandardizeAddressService adrSrv = new AddrService.StandardizeAddressService();
        public MainWindow()
        {
            InitializeComponent();

            List<string> l = new List<string>();
            l.Add("Dev");
            l.Add("EDI");
            l.Add("RM");
            l.Add("Prod");
            l.Add("Live");
            cboxENV.ItemsSource = l;

        }


        /// <summary>
        /// User wants to Standardize some input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStandardize_Click(object sender, RoutedEventArgs e)
        {
            ClearLabels();      // Remove any residual text from a previous click

            string val = cboxENV.SelectedItem as string;
            if ("Dev".Equals ( val ) )
                adrSrv.Url = "http://adrwslb-fhidev.firsthealth.com/StandardizeAddressDEV/StandardizeAddress";  // DEV
            if ("EDI".Equals ( val ) )
                adrSrv.Url = "http://adrwslb-fhitest.cvty.com/StandardizeAddressTEST/StandardizeAddress";       // EDI
            if ("LIVE".Equals ( val ) )
                adrSrv.Url = "http://atrwslb-fhiuat.cvty.com/StandardizeAddressLIVE/StandardizeAddress";        // LIVE
            if ("RM".Equals ( val ) )
                adrSrv.Url = "http://atrwslb-fhiuat.cvty.com/StandardizeAddressRM/StandardizeAddress";          // RM
            if ("Prod".Equals ( val ) )
                adrSrv.Url = "http://intranet.firsthealth.com/StandardizeAddress/StandardizeAddress";           // PROD

            AddrResponse resp = adrSrv.checkAddress(populateAddrTO());
            if (resp.Success)
            {
                populateValidAddr(resp.Addresses[0]);
            }
            else
            {
                lblAddrLine1.Content = resp.FailureDesc;
            }

        }



        /// <summary>
        /// Create an AddressTO, populate with values from the screen and return.
        /// </summary>
        /// <returns></returns>
        private CDQpAddressTO populateAddrTO()
        {
            CDQpAddressTO to = new CDQpAddressTO();
            to.Addr1 = AddrLine1.Text;
            to.Addr2 = AddrLine2.Text;
            to.City = City.Text;
            to.State = State.Text;
            to.Zip = Zip.Text;
            return to;
        }



        /// <summary>
        /// Successful standardization will populate 
        /// </summary>
        /// <param name="addrTO"></param>
        private void populateValidAddr(CDQpAddressTO addrTO)
        {
            lblAddrLine1.Content = addrTO.Addr1;
            lblAddrLine2.Content = addrTO.Addr2;
            lblCity.Content = addrTO.City;
            lblState.Content = addrTO.State;
            lblZip.Content = addrTO.Zip;
            lblConfidence.Content = addrTO.Confidence + "%";
        }



        /// <summary>
        /// Remvoe any text from RESULT LABELS on the screen.
        /// </summary>
        private void ClearLabels()
        {
            lblAddrLine1.Content = "";
            lblAddrLine2.Content = "";
            lblCity.Content = "";
            lblState.Content = "";
            lblZip.Content = "";
        }
    }
}
