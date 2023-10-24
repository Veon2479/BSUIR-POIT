library IEEE;
use IEEE.std_logic_1164.all;
use IEEE.numeric_std.all;


entity Mux_testbench is
end Mux_testbench;

Architecture behaviour of Mux_testbench is

    Component Mux_struct is
        Port (
            A, B, S : in std_logic;
            Z : out std_logic
         );
    end Component;

    Component Mux_beh is
        Port (
            A, B, S : in std_logic;
            Z : out std_logic
         );
    end Component;

    signal ta, tb, ts, tz_s, tz_b : std_logic;

    function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;


begin

    mxb: Mux_beh port map
    (
        A => ta,
        B => tb,
        S => ts,
        Z => tz_b
    );

    mxs: Mux_struct port map
    (
        A => ta,
        B => tb,
        S => ts,
        Z => tz_s
    );

    process
    begin
        for a in 0 to 1 loop
            for b in 0 to 1 loop
                for s in 0 to 1 loop
                    ta <= to_stdlogic(a);
                    tb <= to_stdlogic(b);
                    ts <= to_stdlogic(s);

                    wait for 10 ms;

                    if tz_s /= tz_b then
                        report "Output mismatch at a = " & integer'image(a) & ", b = " & integer'image(b) & ", s = " & integer'image(s);
                        report "z_s = " & std_logic'image(tz_s);
                        report "z_b = " & std_logic'image(tz_b);
                    end if;
                end loop;
            end loop;
        end loop;
        report "testbench completed!";
        wait;
    end process;
end;
