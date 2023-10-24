library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Comb_beh is
    Port (
            X, Y, Z : in std_logic;
            F : out std_logic
         );
end Comb_beh;

architecture Structural of Comb_beh is
begin

    F <= (X and Z) or ((not Y) and Z) or ((not X) and Y and (not Z));

end Structural;
