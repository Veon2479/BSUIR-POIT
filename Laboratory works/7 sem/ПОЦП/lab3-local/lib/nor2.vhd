library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity Nor2 is
    Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
end Nor2;

architecture Behaviour of Nor2 is
begin
    pout <= not (pin1 or pin2);
end architecture;
