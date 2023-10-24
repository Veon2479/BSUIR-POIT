library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity And2 is
    Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
end And2;

architecture Behaviour of And2 is
begin
    pout <= pin1 and pin2;
end architecture;
