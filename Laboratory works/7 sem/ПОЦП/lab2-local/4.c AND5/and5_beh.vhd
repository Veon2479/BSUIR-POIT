library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity And5_beh is
    Port (
            pin : in std_logic_vector(4 downto 0);
            pout : out std_logic
         );
end And5_beh;

architecture Structural of And5_beh is
begin

    pout <= pin(0) and pin(1) and pin(2) and pin(3) and pin(4);

end Structural;
