library IEEE;
use IEEE.std_logic_1164.all;
use IEEE.numeric_std.all;

entity Testbench is
end Testbench;

Architecture behaviour of Testbench is

    Component Bistable is
        Port (
            Q, NQ : out std_logic
         );
    end Component;

    signal q : std_logic := '0';
    signal nq : std_logic := '0';

begin

    bis: Bistable port map (Q => q, NQ => nq);

    process
    begin
        wait for 10 ms;
        report "testbench completed!";
        wait;
    end process;
end;
