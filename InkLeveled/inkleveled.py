#!/usr/bin/env python
#Boa:PyApp:main

#modules ={}

import os
import glob
import cherrypy
import pynklevel
import traceback
from optparse import OptionParser

class InkLevelD():
    def index(self):
        return "It's alive!"
    index.exposed = True


    def GetPrinters(self):
        devices = glob.glob('/dev/lp*')
        devices += glob.glob('/dev/usb/lp*')

        retval  = '<?xml version="1.0" encoding=' + "'UTF-8'?>"
        retval += "<Printers>"
        for device in devices:
                retval += "<Printer>"
                retval += "<Name>"
                retval += str(device)
                retval += "</Name>"
                retval += "<Device>"
                retval += str(device)
                retval += "</Device>"
                retval += "</Printer>"
        retval += "</Printers>"

        return retval
    GetPrinters.exposed = True

    def GetPrinterInk(self,  printer):
        try:
            p = pynklevel.getusbprinterlevels(printer)
            result = '<?xml version="1.0" encoding=' + "'UTF-8'?>"
            result += "<PrinterLevels>"
            for i in range(p.TankCount()) :
                tanktype = p.GetTankType(i);
                result += "<" + tanktype  + ">"
                result += str(p.GetTankLevel(i))
                result += "</" + tanktype + ">"
            result += "</PrinterLevels>"
            return result
        except pynklevel.LevelerException, error:
            return self.ExceptionToXML( error );

    def ExceptionToXML(self, error): 
        result = '<?xml version="1.0" encoding=' + "'UTF-8'?>"
        result += "<Error>"
        result += error.what()
        result += "</Error>"
        return result
    GetPrinterInk.exposed = True

if __name__ == "__main__":
    parser = OptionParser()
    parser.add_option("-l", "--listen", dest="listen", help="Hostname/address to listen on")
    parser.add_option("-p", "--port", dest="port", type="int", help="Port to listen on" )
    (options,args) = parser.parse_args()
    #parser.check_required("-l")
    #parser.check_required("-p")

    if parser.get_option("-l") == None:
        print "-l is required"
        parser.print_help()
        quit(-1)
    if parser.get_option("-p") == None:
        print "-p is required"
        parser.print_help()
        quit(-1)

    root = InkLevelD()
    print( "starting out...")
    # INADDR_ANY: listen on all interfaces

    cherrypy.server.socket_host = options.listen
    cherrypy.server.socket_port = options.port
    cherrypy.quickstart(InkLevelD())
