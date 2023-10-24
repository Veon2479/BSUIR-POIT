library IEEE;
use IEEE.std_logic_1164.all;

entity Rsl is
    Port(
            R, S : in std_logic;
            Q, NQ : out std_logic
        );
end Rsl;

architecture Behavioral of Rsl is

    Component Nor2 is
        Port (
            pin1 : in std_logic;
            pin2 : in std_logic;
            pout : out std_logic
         );
    end Component;

    signal s1, s2 : std_logic;

begin

    nor2_1: Nor2 port map(S, s1, s2);
    nor2_2: Nor2 port map(R, s2, s1);

    Q <= s1;
    NQ <= s2;

end Behavioral;
