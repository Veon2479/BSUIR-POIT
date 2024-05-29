library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity Or2 is
    Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
end Or2;

architecture Behaviour of Or2 is
begin
    pout <= pin1 or pin2;
end architecture;
