----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 09:32:28 PM
-- Design Name: 
-- Module Name: TFF_testbench - Behavioral
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

entity TFF_testbench is
--  Port ( );
end TFF_testbench;

architecture Behavioral of TFF_testbench is
    Component TFF is 
          Port ( 
    CLR, C, T : in std_logic;
    Q : out std_logic
  );
    end component;
    
  function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;
   
   signal t, c, clr, q: std_logic := '0';
begin
u: TFF port map(clr, c, t, q);

main: process
begin
    clr <= '1';
    wait for 5 ns;
    clr <= '0';
    
    for i in 0 to 2 loop

                for d in 0 to 1 loop

                    t <= to_stdlogic(d);
                    wait for 20 ns;
                end loop;

    end loop;
    wait;
end process;

Clkp: process
begin
        c<='0';
        wait for 10 ns;
        c <= '1';
        wait for 10 ns;

end process;

end Behavioral;
