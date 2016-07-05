#include <MFRC522.h>
#include <SPI.h>
#include <Keypad.h>

#define SS_PIN 10
#define  RST_PIN 9

MFRC522 rfid(SS_PIN, RST_PIN);
MFRC522::MIFARE_Key key;

byte inputByte_0;
byte inputByte_1;
byte inputByte_2;
byte inputByte_3;
byte inputByte_4;

int safePin = 2;
const byte ROWS = 4;
const byte COLS = 4;
char keys[ROWS][COLS] =
{
    {'1', '2', '3', 'A'},
    {'4', '5', '6', 'B'},
    {'7', '8', '9', 'C'},
    {'*', '0', '#', 'D'}
};
byte rowPins[ROWS] = { 2, 3, 4, 5 };
byte colPins[COLS] = { A1, A2, A3, A4 };
Keypad keypad = Keypad(makeKeymap(keys), rowPins, colPins, ROWS, COLS);

String activeCard = "Start";
String previousCard = "";

void setup()
{
    pinMode(safePin, INPUT_PULLUP);
    Serial.begin(9600);
    SPI.begin();
    rfid.PCD_Init();
    for(byte i = 0; i < 6; i++)
    {
        key.keyByte[i] = 0xFF;
    }
}

void loop()
{
    handshake();
    readKeys();
    readRFID();
}

void handshake()
{
    if(Serial.available() == 5) 
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
            }
            else
            {
                activeCard = "Start";
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
    }
}

void readKeys()
{
    if (digitalRead(safePin) == HIGH)
    {
        char key = keypad.getKey();
        if(key != NO_KEY)
        {
            Serial.println(key);
        }
    }
}

void readRFID()
{
    if(rfid.PICC_IsNewCardPresent())
    {
        if(rfid.PICC_ReadCardSerial())
        {
            previousCard = "";
            byte PasID[18];
            byte RekeningID[18];
            byte KlantID[18];
            readBlock(62, PasID);
            readBlock(61, RekeningID);
            readBlock(60, KlantID);
            for(int i = 0; i < 3; i++)
            {
                previousCard += char(PasID[i]);
            }
            if(previousCard != activeCard)
            {
                Serial.print("PasID: ");
                Serial.println(previousCard);
                activeCard = previousCard;
            }
            rfid.PICC_HaltA();
            rfid.PCD_StopCrypto1();
        }
    }
}
