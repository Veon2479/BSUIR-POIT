library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity Inv1 is
    Port (
            pin : in std_logic;
            pout : out std_logic
         );
end Inv1;

architecture Behaviour of Inv1 is
begin
    pout <= not pin;
end architecture;
