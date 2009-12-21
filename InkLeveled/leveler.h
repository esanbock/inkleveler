#include <stdexcept>

using namespace std;

class LevelerException : public std::runtime_error
{
private:
	int _errorCode;
public:
	LevelerException(const std::string& what);
	LevelerException(const int code);
	virtual const char* what();
	virtual const int whatCode();
	
	//virtual ~LevelerException() throw();
};
    

class Printer
{
  private:
	ink_level _level;
	
  protected:
    static const char* TankTypes[];
    
  public:
	Printer( struct ink_level level );
	vector<int> GetTanks();
	int GetTankLevel( int tankNumber ) throw( LevelerException );
	const char* Name();
	int TankCount();
	const char* GetTankType( int tankNumber ) throw( LevelerException );
};

Printer getusbprinterlevels( const char* device ) throw( LevelerException );
