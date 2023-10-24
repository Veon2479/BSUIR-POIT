library IEEE;
use IEEE.std_logic_1164.all;
use IEEE.numeric_std.all;


entity Mux_testbench is
end Mux_testbench;

Architecture behaviour of Mux_testbench is

    Component Mux_struct_big is
        Port (
            A1, A2, B1, B2, S : in std_logic;
            Z1, Z2 : out std_logic
         );
    end Component;

    Component Mux_beh_big is
        Port (
            A1, A2, B1, B2, S : in std_logic;
            Z1, Z2 : out std_logic
         );
    end Component;

    signal ta1, ta2, tb1, tb2, ts, tz_s1, tz_s2, tz_b1, tz_b2 : std_logic;

    function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;


begin

    my_mux_beh: Mux_beh_big port map
    (
        A1 => ta1,
        A2 => ta2,
        B1 => tb1,
        B2 => tb2,
        S => ts,
        Z1 => tz_b1,
        Z2 => tz_b2
    );

    my_mux_str: Mux_struct_big port map
    (
        A1 => ta1,
        A2 => ta2,
        B1 => tb1,
        B2 => tb2,
        S => ts,
        Z1 => tz_s1,
        Z2 => tz_s2
    );

    process
    begin

        for a1 in 0 to 1 loop
            for a2 in 0 to 1 loop
                for b1 in 0 to 1 loop
                    for b2 in 0 to 1 loop
                        for s in 0 to 1 loop

                            ta1 <= to_stdlogic(a1);
                            ta2 <= to_stdlogic(a2);
                            tb1 <= to_stdlogic(b1);
                            tb2 <= to_stdlogic(b2);
                            ts <= to_stdlogic(s);

                            wait for 10 ms;

                            if tz_s1 /= tz_b1 or tz_s2 /= tz_b2 then
                                report "Output mismatch!";
                                report "at a1 = " & integer'image(a1) & ", b1 = " & integer'image(b1);
                                report "at a2 = " & integer'image(a2) & ", b2 = " & integer'image(b2);
                                report "at s = " & integer'image(s);
                                report "z_s1 = " & std_logic'image(tz_s1);
                                report "z_s2 = " & std_logic'image(tz_s2);
                                report "z_b1 = " & std_logic'image(tz_b1);
                                report "z_b2 = " & std_logic'image(tz_b2);
                            end if;



                        end loop;
                    end loop;
                end loop;
            end loop;
        end loop;

        report "testbench completed!";
        wait;
    end process;
end;
