library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Mux_beh is
    Port (
            A, B, S : in std_logic;
            Z : out std_logic
         );
end Mux_beh;

architecture Structural of Mux_beh is
begin

    Z <= (A and (not S)) or (B and S);

end Structural;
