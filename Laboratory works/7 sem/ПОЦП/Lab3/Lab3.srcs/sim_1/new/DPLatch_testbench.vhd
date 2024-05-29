----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 09:03:20 PM
-- Design Name: 
-- Module Name: DPLatch_testbench - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity DPLatch_testbench is
--  Port ( );
end DPLatch_testbench;

architecture Behavioral of DPLatch_testbench is
Component DPLatch is 
    Port (
        ps, d, e: in std_logic;
        q : out std_logic
    );    
    end component;
    signal ps, d, e, q : std_logic;
    
       function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;
begin

u: DPLatch port map (ps, d, e, q);

Main: process
begin
    report "Testbench started!";
    for i in 0 to 1 loop 
        for j in 0 to 1 loop 
            for k in 0 to 1 loop 
                ps <= to_stdlogic(i);
                d <= to_stdlogic(j);
                e <= to_stdlogic(k);
                wait for 10 ns;
             

            end loop;
        end loop;
    end loop;
    report "Testbench finished";
    wait;
end process;

end Behavioral;
