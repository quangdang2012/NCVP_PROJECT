using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using System.IO;

namespace BoxIdPrinter
{
    public class TfPrint
    {
        // バーコードプリント機能
        public static void printBarCode(string boxId)
        {
            long rtn;
            int x, y, BarHeight;
            string code_128a = boxId;
            string printerName = "SEWOO Label Printer";

            /* 1. LK_OpenPrinter() */
            if (LKBPRINT.LK_OpenPrinter(printerName) != LKBPRINT.LK_SUCCESS) { return; }

            /* 2. LK_SetupPrinter() */
            rtn = LKBPRINT.LK_SetupPrinter("101.6", 	// 10~104 (Unit is mm)
                "30", 		// 5~350 (Unit is mm)
                0,				// 0=Label with Gap, 1=Label with Black Mark, 2=Label with Continuous.
                "3.1",			// if(MediaType==0) <GapHeight> else <BlackMarkHeight>. (Unit is mm)
                "0",			// if(MediaType==0) <not used> else <distance from BlackMark to perforation>. (Unit is mm)
                8,				// 0 ~ 15
                6,				// 2 ~ 6 (Unit is Inch)
                2				// 1 ~ 9999 copies
                );

            if (rtn != LKBPRINT.LK_SUCCESS) { LKBPRINT.LK_ClosePrinter(); return; }

            /* 3-1. page 1 test */
            LKBPRINT.LK_StartPage();
            BarHeight = 12 * 8;	// 12mm

            x = 30 * 8;
            y = 7 * 8;
            LKBPRINT.LK_PrintBarCode(x, y, 0, "1A", 2, 5, BarHeight, 1, code_128a);

            LKBPRINT.LK_EndPage();

            /* 4. LK_ClosePrinter() */
            LKBPRINT.LK_ClosePrinter();
        }

        // ビットマッププリント機能
        public static void printBitmap(string datecdFile)
        {
            long rtn;
            int x, y;
            string printerName = "SEWOO Label Printer";

            /* 1. LK_OpenPrinter() */
            if (LKBPRINT.LK_OpenPrinter(printerName) != LKBPRINT.LK_SUCCESS) { return; }

            /* 2. LK_SetupPrinter() */
            rtn = LKBPRINT.LK_SetupPrinter("101.6", 	// 10~104 (Unit is mm)
                "30", 		// 5~350 (Unit is mm)
                0,				// 0=Label with Gap, 1=Label with Black Mark, 2=Label with Continuous.
                "3.1",			// if(MediaType==0) <GapHeight> else <BlackMarkHeight>. (Unit is mm)
                "0",			// if(MediaType==0) <not used> else <distance from BlackMark to perforation>. (Unit is mm)
                8,				// 0 ~ 15
                6,				// 2 ~ 6 (Unit is Inch)
                3				// 1 ~ 9999 copies
                );

            if (rtn != LKBPRINT.LK_SUCCESS) { LKBPRINT.LK_ClosePrinter(); return; }

            /* 3-1. page 1 test */
            x = 16 * 8;
            y = 0 * 8;
            LKBPRINT.LK_StartPage();
            LKBPRINT.LK_PrintBMP(x, y, datecdFile);
            LKBPRINT.LK_EndPage();

            /* 4. LK_ClosePrinter() */
            LKBPRINT.LK_ClosePrinter();
        }

    }

}
