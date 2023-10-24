library IEEE;
use IEEE.std_logic_1164.all;
use IEEE.numeric_std.all;

entity Testbench is
end Testbench;

Architecture behaviour of Testbench is

    Component Rsl is
        Port (
                R, S : in std_logic;
                Q, NQ : out std_logic
             );
    end Component;

begin

    process
    begin

        report "Testbench completed";
        wait;
    end process;

end;
