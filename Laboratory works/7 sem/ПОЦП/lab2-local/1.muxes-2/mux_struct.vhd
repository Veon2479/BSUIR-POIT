library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Mux_struct is
    Port (
            A, B, S : in std_logic;
            Z : out std_logic
         );
end Mux_struct;

architecture Structural of Mux_struct is
    Component Inv1 is
        Port (
            pin : in std_logic;
            pout : out std_logic
         );
    end Component;

    Component And2 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
    end Component;

    Component Or2 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
    end Component;

    signal NS, ANS, BS : std_logic;

begin

    inv1_1: Inv1 port map (
        pin => S,
        pout => NS
    );

    and2_1: And2 port map (
        pin1 => A,
        pin2 => NS,
        pout => ANS
    );

    and2_2: And2 port map (
        pin1 => B,
        pin2 => S,
        pout => BS
    );

    or2_1: Or2 port map (
        pin1 => ANS,
        pin2 => BS,
        pout => Z
    );

end Structural;
