library IEEE;
use IEEE.std_logic_1164.all;
use IEEE.numeric_std.all;


entity Dmux_testbench is
end Dmux_testbench;

Architecture behaviour of Dmux_testbench is

    Component Dmux_struct is
        Port (
            A, S0, S1 : in std_logic;
            B0, B1, B2, B3 : out std_logic
         );
    end Component;

    Component Dmux_beh is
        Port (
            A, S0, S1 : in std_logic;
            B0, B1, B2, B3 : out std_logic
         );
    end Component;

    signal ta, ts0, ts1 : std_logic;
    signal tbs, tbb : std_logic_vector (3 downto 0);

    function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;


begin

    my_demux_beh: Dmux_beh port map
    (
        A => ta,
        S0 => ts0,
        S1 => ts1,
        B0 => tbb(0),
        B1 => tbb(1),
        B2 => tbb(2),
        B3 => tbb(3)
    );

    my_demux_str: Dmux_struct port map
    (
        A => ta,
        S0 => ts0,
        S1 => ts1,
        B0 => tbs(0),
        B1 => tbs(1),
        B2 => tbs(2),
        B3 => tbs(3)
    );

    process
    begin

        for a in 0 to 1 loop
            for s0 in 0 to 1 loop
                for s1 in 0 to 1 loop

                            ta <= to_stdlogic(a);
                            ts0 <= to_stdlogic(s0);
                            ts1 <= to_stdlogic(s1);

                            wait for 10 ms;

                            assert (tbs = tbb)
                            report "Output mismatch!";

                end loop;
            end loop;
        end loop;

        report "testbench completed!";
        wait;
    end process;
end;
