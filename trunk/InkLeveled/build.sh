g++ InkPlus.cpp leveler.cpp -L /usr/lib -l inklevel -o InkPlus
swig -python -c++ -module pynklevel leveler.h
g++ -shared -fPIC -include leveler.cpp -include inklevel.h leveler_wrap.cxx  -l inklevel -I /usr/include/python2.5 -L /usr/lib/python2.5 -o _pynklevel.so

