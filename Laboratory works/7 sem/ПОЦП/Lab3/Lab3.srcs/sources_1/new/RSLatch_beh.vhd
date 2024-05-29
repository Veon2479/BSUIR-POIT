----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/04/2023 12:09:23 AM
-- Design Name: 
-- Module Name: RSLatch_beh - Behavioral
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

entity RSLatch_beh is
    Port ( R : in STD_LOGIC;
           S : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);
end RSLatch_beh;

architecture Behavioral of RSLatch_beh is
    signal v : std_logic;
begin
    Main: process(R, S) 
    begin 
        if (R = '1') then
            v <= '0';
        elsif (S = '1') then
            v <= '1';
        end if; 
    end process;
    
    Q <= v;
    NQ <= not v;

end Behavioral;
