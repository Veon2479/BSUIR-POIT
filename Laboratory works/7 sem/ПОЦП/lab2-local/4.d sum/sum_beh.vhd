library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Sum_beh is
    Port (
            A, B : in std_logic_vector (1 downto 0);
            ICF : in std_logic;
            CF : out std_logic;
            S : out std_logic_vector (1 downto 0)
         );
end Sum_beh;

architecture Structural of Sum_beh is

    Component Sum1_beh is
        Port (
            A, B, ICF : in std_logic;
            S, CF : out std_logic
         );
    end Component;

    signal in_cf : std_logic;

begin

    sums0: Sum1_beh port map
    (
        A => A(0),
        B => B(0),
        ICF => ICF,
        S => S(0),
        CF => in_cf
    );

    sums1: Sum1_beh port map
    (
        A => A(1),
        B => B(1),
        ICF => in_cf,
        S => S(1),
        CF => CF
    );


end Structural;
