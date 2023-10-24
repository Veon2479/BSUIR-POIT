library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Dmux_beh is
    Port (
            A, S0, S1 : in std_logic;
            B0, B1, B2, B3 : out std_logic
         );
end Dmux_beh;

architecture Structural of Dmux_beh is
begin

    B0 <= (not S0) and (not S1) and A;
    B1 <= (not S0) and S1 and A;
    B2 <= S0 and (not S1) and A;
    B3 <= S0 and S1 and A;

end Structural;
