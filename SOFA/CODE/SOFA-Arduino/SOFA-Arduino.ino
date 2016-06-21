#include <Keypad.h>
#include <MFRC522.h>
#include <SPI.h>
#define  RST_PIN 9
#define SS_PIN 10
int safePin = 2;
MFRC522 mfrc522(SS_PIN, RST_PIN);
MFRC522::MIFARE_Key key;
byte nuidPICC[3];
const byte ROWS = 4; //four rows
const byte COLS = 4; //three columns
char keys[ROWS][COLS] = {
  {'1', '2', '3', 'A'},
  {'4', '5', '6', 'B'},
  {'7', '8', '9', 'C'},
  {'*', '0', '#', 'D'}
};
byte rowPins[ROWS] = { 2, 3, 4, 5 };
byte colPins[COLS] = { A1, A2, A3, A4 };

Keypad keypad = Keypad( makeKeymap(keys), rowPins, colPins, ROWS, COLS );
byte inputByte_0;
byte inputByte_1;
byte inputByte_2;
byte inputByte_3;
byte inputByte_4;
int timeout = 0;
bool newcard = false;
void setup()
{
  pinMode(safePin, INPUT_PULLUP);
  Serial.begin(9600);
  SPI.begin();
  mfrc522.PCD_Init();        // Init MFRC522 card
  //Serial.println("Print block 0 of a MIFARE PICC ");
  for (byte i = 0; i < 6; i++)
  {
    key.keyByte[i] = 0xFF;//keyByte is defined in the "MIFARE_Key" 'struct' definition in the .h file of the library
  }
}

void dump_byte_array(byte *buffer, byte bufferSize) {
  for (byte i = 0; i < bufferSize; i++) {
    Serial.print(buffer[i] < 0x10 ? " 0" : "");
    Serial.print(buffer[i], HEX);
  }
}

void loop()
{
  handshake();
  rfidAndKey();
}

void rfidAndKey() {
  timeout++;
  if (timeout == 15)
  {
    newcard = true;
  }
  if (mfrc522.PICC_IsNewCardPresent())
  {
    timeout = 0;
    if (newcard)
    {
      if (mfrc522.PICC_ReadCardSerial())
      {
        byte PasID[18];
        byte RekeningID[18];
        byte KlantID[18];
        readBlock(62, PasID);
        readBlock(61, RekeningID);
        readBlock(60, KlantID);
        Serial.write("PasID: ");
        for (int i = 0; i < 3; i++)
        {
          Serial.write(PasID[i]);
        }
        Serial.write("\n");
        /*
        Serial.write("RekeningID: ");
        for (int i = 0; i < 8; i++)
        {
          Serial.write(RekeningID[i]);
        }
        Serial.write("\n");
        Serial.write("KlantID: ");
        for (int i = 0; i < 4; i++)
        {
          Serial.write(KlantID[i]);
        }
        Serial.write("\n");
        */
        newcard = false;
        delay(1000);
        asm volatile ("  jmp 0");
      }
    }
  }

  if (digitalRead(safePin) == HIGH)
  {
    char key = keypad.getKey();
    if (key != NO_KEY)
    {
      Serial.println(key);
    }
  }
  else
  {
    Serial.println("open");
    delay(5000);
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
