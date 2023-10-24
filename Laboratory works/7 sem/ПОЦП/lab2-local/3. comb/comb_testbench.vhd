library IEEE;
use IEEE.std_logic_1164.all;
use IEEE.numeric_std.all;


entity Comb_testbench is
end Comb_testbench;

Architecture behaviour of Comb_testbench is

    Component Comb_struct is
        Port (
            X, Y, Z : in std_logic;
            F : out std_logic
         );
    end Component;

    Component Comb_beh is
        Port (
            X, Y, Z : in std_logic;
            F : out std_logic
         );
    end Component;

    signal tx, ty, tz, tfs, tfb : std_logic;

    function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;


begin

    mxb: Comb_beh port map
    (
        X => tx,
        Y => ty,
        Z => tz,
        F => tfb
    );

    mxs: Comb_struct port map
    (
        X => tx,
        Y => ty,
        Z => tz,
        F => tfs
    );

    process
    begin
        for x in 0 to 1 loop
            for y in 0 to 1 loop
                for z in 0 to 1 loop
                    tx <= to_stdlogic(x);
                    ty <= to_stdlogic(y);
                    tz <= to_stdlogic(z);

                    wait for 10 ms;

                    if tfb /= tfs then
                        report "Output mismatch at x = " & integer'image(x) & ", y = " & integer'image(y) & ", z = " & integer'image(z);
                        report "f_s = " & std_logic'image(tfs);
                        report "f_b = " & std_logic'image(tfb);
                    end if;
                end loop;
            end loop;
        end loop;
        report "testbench completed!";
        wait;
    end process;
end;
