library IEEE;
use IEEE.std_logic_1164.all;
use IEEE.numeric_std.all;


entity And5_testbench is
end And5_testbench;

Architecture behaviour of And5_testbench is

    Component And5_struct is
        Port (
            pin : in std_logic_vector(4 downto 0);
            pout : out std_logic
         );
    end Component;

    Component And5_beh is
        Port (
            pin : in std_logic_vector(4 downto 0);
            pout : out std_logic
         );
    end Component;

    signal ta_b, ta_s : std_logic;
    signal ta : std_logic_vector (4 downto 0);

    function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;


begin

    mxb: And5_beh port map
    (
        pin => ta,
        pout => ta_b
    );

    mxs: And5_struct port map
    (
        pin => ta,
        pout => ta_s
    );

    process
    begin
        for i in 0 to 31 loop
            ta <= std_logic_vector(to_unsigned(i, ta'length));

            wait for 10 ms;

            if ta_b /= ta_s then
                report "Output mismatch at i = " & integer'image(i);
                report "ta_b = " & std_logic'image(ta_b);
                report "ta_s = " & std_logic'image(ta_s);
            end if;

        end loop;
        report "testbench completed!";
        wait;
    end process;
end;
