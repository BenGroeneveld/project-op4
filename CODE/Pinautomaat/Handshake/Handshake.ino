#include <Keypad.h>
#include <AddicoreRFID.h>
#include <SPI.h>

AddicoreRFID myRFID;
const int chipSelectPin = 10;
const int NRSTPD = 9;
int maxLength = 16;

int iCheck;

unsigned char activeCard = '0';
unsigned char previousCard = '0';

const byte ROWS = 4;
const byte COLS = 4;
char keys[ROWS][COLS] =
{
    {'1','2','3','A'},
    {'4','5','6','B'},
    {'7','8','9','C'},
    {'*','0','#','D'}
};

byte rowPins[ROWS] = { 2, 3, 4, 5 };
byte colPins[COLS] = { A1, A2, A3, A4 };

Keypad kpad = Keypad(makeKeymap(keys), rowPins, colPins, ROWS, COLS);
byte inputByte_0;
byte inputByte_1;
byte inputByte_2;
byte inputByte_3;
byte inputByte_4;

void setup()
{
      Serial.begin(9600);
      SPI.begin();
      pinMode(chipSelectPin,OUTPUT);
      digitalWrite(chipSelectPin, LOW);
      pinMode(NRSTPD,OUTPUT);
      digitalWrite(NRSTPD, HIGH);
      myRFID.AddicoreRFID_Init();
}

void loop()
{
      handshake();
      rfidInput();
      keypadInput();
}

void rfidInput()
{
      unsigned char i, tmp, checksum1;
      unsigned char status;
      unsigned char str[maxLength];
      status = myRFID.AddicoreRFID_Request(PICC_REQIDL, str);
      status = myRFID.AddicoreRFID_Anticoll(str);
      if(status == MI_OK)
      {
            if(activeCard != str[0])
            {
                  Serial.print("ID");
                  Serial.print(str[0], DEC);
                  previousCard = str[0];
                  Serial.println();
            }
      }
      myRFID.AddicoreRFID_Halt();
}

void keypadInput()
{
      char key = kpad.getKey();
      if (key)
      {
          Serial.println(key);
      }
}

void handshake()
{
      if (Serial.available() == 5) 
      {
            inputByte_0 = Serial.read();
            inputByte_1 = Serial.read();      
            inputByte_2 = Serial.read();
            inputByte_3 = Serial.read();
            inputByte_4 = Serial.read();   
      }
    
      if(inputByte_0 == 16)
      {
           if(inputByte_1 == 127)
           {
                  if(inputByte_3 == 255)
                  {
                        activeCard = previousCard;
                        Serial.print("activeCard: ");
                        Serial.println(activeCard);
                  }
                  else
                  {
                        activeCard = '0';
                  }
           }
           else if(inputByte_1 == 128)
           {
                  Serial.print("Arduino");
           }
           
            inputByte_0 = 0;
            inputByte_1 = 0;
            inputByte_2 = 0;
            inputByte_3 = 0;
            inputByte_4 = 0;
            /*Serial.print(" is ready to receive");*/
      }
}
