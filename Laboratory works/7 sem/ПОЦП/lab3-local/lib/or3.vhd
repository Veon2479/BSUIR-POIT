library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity Or3 is
    Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pin3 : in std_logic;
            pout : out std_logic
         );
end Or3;

architecture Behaviour of Or3 is
begin
    pout <= pin1 or pin2 or pin3;
end architecture;
