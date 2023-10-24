library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Mux_struct_big is
    Port (
            A1, A2, B1, B2, S : in std_logic;
            Z1, Z2 : out std_logic
         );

end Mux_struct_big;

architecture Structural of Mux_struct_big is

    Component Mux_struct is
        Port (
            A, B, S : in std_logic;
            Z : out std_logic
        );
    end Component;

begin

    mux0: Mux_struct port map
    (
        A => A1,
        B => B1,
        S => S,
        Z => Z1
    );

    mux1: Mux_struct port map
    (
        A => A2,
        B => B2,
        S => S,
        Z => Z2
    );

end Structural;
