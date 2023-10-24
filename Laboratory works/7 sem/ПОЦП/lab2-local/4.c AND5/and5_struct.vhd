library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity And5_struct is
    Port (
        pin : in std_logic_vector(4 downto 0);
        pout : out std_logic
    );
end And5_struct;

architecture Structural of And5_struct is

    Component And2 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
    end Component;

    signal tmp : std_logic_vector (3 downto 0);

begin

    GEN_0: AND2 port map (pin1 => pin(0), pin2 => pin(1), pout => tmp(0));
    SCH: FOR J in 1 to 3 GENERATE
        GEN_J: AND2 port map (pin1 => tmp(J-1), pin2 => pin(J+1), pout => tmp(J));
    end GENERATE;
    pout <= tmp(3);

end Structural;
