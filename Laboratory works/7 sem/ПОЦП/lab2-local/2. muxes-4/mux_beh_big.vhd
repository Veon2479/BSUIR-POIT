library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Mux_beh_big is
    Port (
            A1, A2, B1, B2, S : in std_logic;
            Z1, Z2 : out std_logic
         );
end Mux_beh_big;

architecture Structural of Mux_beh_big is
begin

    Z1 <= (A1 and (not S)) or (B1 and S);
    Z2 <= (A2 and (not S)) or (B2 and S);


end Structural;
