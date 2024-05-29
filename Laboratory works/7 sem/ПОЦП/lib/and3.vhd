library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity And3 is
    Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pin3 : in std_logic;
            pout : out std_logic
         );
end And3;

architecture Behaviour of And3 is
begin
    pout <= pin1 and pin2 and pin3;
end architecture;
