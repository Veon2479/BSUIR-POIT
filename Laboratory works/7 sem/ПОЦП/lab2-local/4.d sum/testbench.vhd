library IEEE;
use IEEE.std_logic_1164.all;
use IEEE.numeric_std.all;


entity Testbench is
end Testbench;

Architecture behaviour of Testbench is

    function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;

    function to_string(v: std_logic_vector) return string is
        variable result: string(v'length-1 downto 0);
    begin
        for i in v'range loop
            result(i) := character'VALUE(std_logic'IMAGE(v(i)));
        end loop;
        return result;
    end function;

    Component Sum_beh is
        Port (
            A, B : in std_logic_vector (1 downto 0);
            ICF : in std_logic;
            CF : out std_logic;
            S : out std_logic_vector (1 downto 0)
         );
    end Component;

    Component Sum_struct is
        Port (
            A, B : in std_logic_vector (1 downto 0);
            ICF : in std_logic;
            CF : out std_logic;
            S : out std_logic_vector (1 downto 0)
         );
    end Component;

    signal ICF, CF_s, CF_b : std_logic;
    signal A, B, S_s, S_b : std_logic_vector (1 downto 0);

begin

    msb: Sum_beh port map
    (
        A => A,
        B => B,
        ICF => ICF,
        S => S_b,
        CF => CF_b
    );

    myss: Sum_struct port map
    (
        A => A,
        B => B,
        ICF => ICF,
        S => S_s,
        CF => CF_s
    );

    process
    begin
        for iicf in 0 to 1 loop
            for ia in 0 to 3 loop
                for ib in 0 to 3 loop

                    ICF <= to_stdlogic(iicf);
                    A <= std_logic_vector(to_unsigned(ia, A'length));
                    B <= std_logic_vector(to_unsigned(ib, B'length));

                    wait for 10 ms;

                    if (CF_s /= CF_b) or (S_s /= S_b) then
                        report "Output mismatch at a = " & integer'image(ia) & ", b = " & integer'image(ib) & ", icf = " & integer'image(iicf);
                        report "cf_s = " & std_logic'image(CF_s) & "s_s is " & to_string(S_s);
                        report "cf_b = " & std_logic'image(CF_b) & "s_b is " & to_string(S_b);
                    end if;

                end loop;
            end loop;
        end loop;
        report "testbench completed!";
        wait;
    end process;
end;
