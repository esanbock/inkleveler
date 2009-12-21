#include <string>
#include <vector>
#include <stdexcept>
#include <sstream>

extern "C"
{
#include <inklevel.h>
}

#include "leveler.h"

using namespace std;

LevelerException::LevelerException(const std::string& what)
  :runtime_error(what)
{
  _errorCode = 0;
}
LevelerException::LevelerException(const int code)
  :runtime_error("See code")
{
  _errorCode = code;
}

/*LevelerException::~LevelerException() throw();
{
}*/

const int LevelerException::whatCode()
{
  return _errorCode;
}

const char* LevelerException::what()
{
  if( _errorCode == 0 )
    return runtime_error::what();
  
  switch( _errorCode )
  {
    case ERROR:
      return "ERROR";
    case DEV_PARPORT_INACCESSIBLE:
      return "DEV_PARPORT_INACCESSIBLE";
    case DEV_LP_INACCESSIBLE:
      return "DEV_LP_INACCESSIBLE";
    case COULD_NOT_GET_DEVICE_ID:
      return "COULD_NOT_GET_DEVICE_ID";
    case DEV_USB_LP_INACCESSIBLE:
      return "DEV_USB_LP_INACCESSIBLE";
    case UNKNOWN_PORT_SPECIFIED:
      return "UNKNOWN_PORT_SPECIFIED";
    case NO_PRINTER_FOUND:
      return "NO_PRINTER_FOUND";
    case NO_DEVICE_CLASS_FOUND:
      return "NO_DEVICE_CLASS_FOUND";
    case NO_CMD_TAG_FOUND:
      return "NO_CMD_TAG_FOUND";
    case PRINTER_NOT_SUPPORTED:
      return "PRINTER_NOT_SUPPORTED";
    case NO_INK_LEVEL_FOUND:
      return "NO_INK_LEVEL_FOUND";
    case COULD_NOT_WRITE_TO_PRINTER:
      return "COULD_NOT_WRITE_TO_PRINTER";
    case COULD_NOT_READ_FROM_PRINTER:
      return "COULD_NOT_READ_FROM_PRINTER";
    case COULD_NOT_PARSE_RESPONSE_FROM_PRINTER:
      return "COULD_NOT_PARSE_RESPONSE_FROM_PRINTER";
    case COULD_NOT_GET_CREDIT:
      return "COULD_NOT_GET_CREDIT";
    case DEV_CUSTOM_USB_INACCESSIBLE:
      return "DEV_CUSTOM_USB_INACCESSIBLE";
    case BJNP_URI_INVALID:
      return "BJNP_URI_INVALID";
    case BJNP_INVALID_HOSTNAME:
      return "BJNP_INVALID_HOSTNAME";
    default:
      return "UNKNOWN";
  }

}
    

Printer::Printer( struct ink_level level )
{
	_level = level;
}

vector<int> Printer::GetTanks()
{
	vector<int> tanks;

	for( int i=0; i < MAX_CARTRIDGE_TYPES; i++ )
	{
		if( _level.levels[i][0] !=  CARTRIDGE_NOT_PRESENT )
			tanks.push_back( (int) _level.levels[i][0] );		
	}
	return tanks;
}

int Printer::GetTankLevel( int tankNumber ) throw( LevelerException )
{
       if( tankNumber > GetTanks().size() )
                throw LevelerException("tank does not exist");

	return _level.levels[tankNumber][1];
}

const char* Printer::Name()
{
	return _level.model;
}

int Printer::TankCount()
{
	return GetTanks().size();
}

const char* Printer::GetTankType( int tankNumber ) throw( LevelerException )
{
	if( tankNumber > GetTanks().size() )
		throw LevelerException("tank does not exist");

	int tankType = GetTanks()[ tankNumber ];
	return TankTypes[tankType];
	
}


Printer getusbprinterlevels( const char* deviceName ) throw( LevelerException )
{
	int result = 0;

	ink_level lev;
	result = get_ink_level( CUSTOM_USB, deviceName, 0, &lev );

	if( result != 0 )
	{
		throw LevelerException( result );
	}

	return Printer(lev);
}

const char* Printer::TankTypes[] = { "CARTRIDGE_NOT_PRESENT",
      "CARTRIDGE_BLACK",
      "CARTRIDGE_COLOR",
      "CARTRIDGE_PHOTO",
      "CARTRIDGE_CYAN",
      "CARTRIDGE_MAGENTA",
      "CARTRIDGE_YELLOW",
      "CARTRIDGE_PHOTOBLACK",
      "CARTRIDGE_PHOTOCYAN",
      "CARTRIDGE_PHOTOMAGENTA",
      "CARTRIDGE_PHOTOYELLOW",
      "CARTRIDGE_RED",
      "CARTRIDGE_GREEN",
      "CARTRIDGE_BLUE",
      "CARTRIDGE_LIGHTBLACK",
      "CARTRIDGE_LIGHTCYAN",
      "CARTRIDGE_LIGHTMAGENTA",
      "CARTRIDGE_LIGHTLIGHTBLACK",
      "CARTRIDGE_MATTEBLACK",
      "CARTRIDGE_GLOSSOPTIMIZER",
      "CARTRIDGE_UNKNOWN",
      "CARTRIDGE_KCM",
      "CARTRIDGE_GGK",
      "CARTRIDGE_KCMY",
      "CARTRIDGE_LCLM",
      "CARTRIDGE_YM",
      "CARTRIDGE_CK",
      "CARTRIDGE_LGPK",
      "CARTRIDGE_LG",
      "CARTRIDGE_G",
      "CARTRIDGE_PG",
      "CARTRIDGE_WHITE"
    };

