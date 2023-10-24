library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity Sum1_beh is
    Port (
            A, B, ICF : in std_logic;
            S, CF : out std_logic
         );
end Sum1_beh;

architecture Behaviour of Sum1_beh is

    signal IN_CF: std_logic;

begin

    IN_CF <= (A and B) or (A and ICF) or (B and ICF);

    CF <= IN_CF;

    S <= ((not IN_CF) and (A or B or ICF)) or (A and B and ICF);


end Behaviour;
